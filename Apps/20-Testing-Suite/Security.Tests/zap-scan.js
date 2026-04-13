const zap = require('zap-api');

const ZAP_URL = 'http://localhost:8080';
const TARGET_URL = 'http://localhost:8080';

async function runSecurityScan() {
    console.log('Starting ZAP security scan...');
    
    // Start ZAP session
    await zap.startSession();
    
    // Spider the target
    console.log('Spidering target...');
    await zap.spider.scan(TARGET_URL);
    
    // Wait for spider to complete
    await zap.waitForSpiderCompletion();
    
    // Run active scan
    console.log('Running active scan...');
    await zap.ascan.scan(TARGET_URL);
    
    // Wait for scan to complete
    await zap.waitForScanCompletion();
    
    // Get alerts
    const alerts = await zap.core.alerts();
    
    // Report findings
    console.log('\n=== Security Scan Results ===');
    console.log(`Total alerts found: ${alerts.length}`);
    
    const highAlerts = alerts.filter(a => a.risk === 'High');
    const mediumAlerts = alerts.filter(a => a.risk === 'Medium');
    const lowAlerts = alerts.filter(a => a.risk === 'Low');
    
    console.log(`High risk: ${highAlerts.length}`);
    console.log(`Medium risk: ${mediumAlerts.length}`);
    console.log(`Low risk: ${lowAlerts.length}`);
    
    if (highAlerts.length > 0) {
        console.log('\nHigh Risk Alerts:');
        highAlerts.forEach(alert => {
            console.log(`  - ${alert.name}: ${alert.url}`);
        });
        process.exit(1);
    }
    
    console.log('\nSecurity scan completed successfully!');
}

runSecurityScan().catch(console.error);