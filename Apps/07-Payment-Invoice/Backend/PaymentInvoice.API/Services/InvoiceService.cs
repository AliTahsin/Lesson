using AutoMapper;
using PaymentInvoice.API.Models;
using PaymentInvoice.API.DTOs;
using PaymentInvoice.API.Repositories;
using System.Text;

namespace PaymentInvoice.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return invoice != null ? _mapper.Map<InvoiceDto>(invoice) : null;
        }

        public async Task<InvoiceDto> GetInvoiceByReservationAsync(int reservationId)
        {
            var invoice = await _invoiceRepository.GetByReservationIdAsync(reservationId);
            return invoice != null ? _mapper.Map<InvoiceDto>(invoice) : null;
        }

        public async Task<List<InvoiceDto>> GetInvoicesByCustomerAsync(int customerId)
        {
            var invoices = await _invoiceRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<List<InvoiceDto>>(invoices);
        }

        public async Task<InvoiceDto> GenerateInvoiceAsync(int reservationId, int customerId)
        {
            var existingInvoice = await _invoiceRepository.GetByReservationIdAsync(reservationId);
            if (existingInvoice != null)
                return _mapper.Map<InvoiceDto>(existingInvoice);

            var invoice = await _invoiceRepository.GenerateInvoiceAsync(reservationId, customerId);
            
            // Send to GIB
            await SendEInvoiceToGIBAsync(invoice.Id);
            
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                throw new Exception("Invoice not found");

            // Simulate PDF generation
            var html = GenerateInvoiceHtml(invoice);
            var pdfBytes = Encoding.UTF8.GetBytes(html); // In real implementation, use DinkToPdf
            
            return await Task.FromResult(pdfBytes);
        }

        public async Task<bool> SendEInvoiceToGIBAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                return false;

            try
            {
                // Simulate GIB API call
                await Task.Delay(500);
                
                var random = new Random();
                var isSuccess = random.Next(0, 10) < 9; // 90% success rate
                
                if (isSuccess)
                {
                    invoice.GIBStatus = "Sent";
                    invoice.EInvoiceNumber = $"EINV-{DateTime.Now.Ticks}";
                    await _invoiceRepository.UpdateAsync(invoice);
                    return true;
                }
                else
                {
                    invoice.GIBStatus = "Failed";
                    invoice.GIBErrorMessage = "GIB connection timeout";
                    await _invoiceRepository.UpdateAsync(invoice);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending e-invoice to GIB");
                invoice.GIBStatus = "Failed";
                invoice.GIBErrorMessage = ex.Message;
                await _invoiceRepository.UpdateAsync(invoice);
                return false;
            }
        }

        private string GenerateInvoiceHtml(Invoice invoice)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Fatura {invoice.InvoiceNumber}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 40px; }}
                    .header {{ text-align: center; margin-bottom: 30px; }}
                    .invoice-title {{ font-size: 24px; font-weight: bold; }}
                    .invoice-number {{ font-size: 16px; color: #666; }}
                    .section {{ margin-bottom: 20px; }}
                    .section-title {{ font-size: 18px; font-weight: bold; margin-bottom: 10px; }}
                    table {{ width: 100%; border-collapse: collapse; }}
                    th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                    th {{ background-color: #f2f2f2; }}
                    .total {{ text-align: right; margin-top: 20px; }}
                    .footer {{ text-align: center; margin-top: 40px; font-size: 12px; color: #666; }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <div class='invoice-title'>FATURA</div>
                    <div class='invoice-number'>No: {invoice.InvoiceNumber}</div>
                    {(!string.IsNullOrEmpty(invoice.EInvoiceNumber) ? $"<div class='invoice-number'>e-Fatura No: {invoice.EInvoiceNumber}</div>" : "")}
                </div>

                <div class='section'>
                    <div class='section-title'>Fatura Bilgileri</div>
                    <div>Tarih: {invoice.IssueDate:dd/MM/yyyy}</div>
                    <div>Son Ödeme Tarihi: {invoice.DueDate:dd/MM/yyyy}</div>
                </div>

                <div class='section'>
                    <div class='section-title'>Müşteri Bilgileri</div>
                    <div>{invoice.CustomerName}</div>
                    <div>Vergi No: {invoice.CustomerTaxId}</div>
                    {(!string.IsNullOrEmpty(invoice.CustomerTaxOffice) ? $"<div>Vergi Dairesi: {invoice.CustomerTaxOffice}</div>" : "")}
                    {(!string.IsNullOrEmpty(invoice.CustomerAddress) ? $"<div>Adres: {invoice.CustomerAddress}</div>" : "")}
                </div>

                <div class='section'>
                    <div class='section-title'>Fatura Kalemleri</div>
                    <table>
                        <thead>
                            <tr><th>Açıklama</th><th>Adet</th><th>Birim Fiyat</th><th>KDV</th><th>Toplam</th></tr>
                        </thead>
                        <tbody>
                            {string.Join("", invoice.Items.Select(item => $@"
                            <tr>
                                <td>{item.Description}</td>
                                <td>{item.Quantity}</td>
                                <td>{item.UnitPrice:C}</td>
                                <td>%{item.TaxRate}</td>
                                <td>{item.TotalPrice:C}</td>
                            </tr>"))}
                        </tbody>
                    </table>
                </div>

                <div class='total'>
                    <div>Ara Toplam: {invoice.SubTotal:C}</div>
                    <div>KDV (%{invoice.TaxRate}): {invoice.TaxAmount:C}</div>
                    <div><strong>Genel Toplam: {invoice.TotalAmount:C}</strong></div>
                </div>

                <div class='footer'>
                    <div>Bu fatura yasal bir belgedir. e-Fatura onaylıdır.</div>
                    <div>GİB entegrasyonu ile düzenlenmiştir.</div>
                </div>
            </body>
            </html>";
        }
    }
}