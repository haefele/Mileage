#Client settings

**Mileage/EmbeddedDatabaseName**  
Where (relative to the application .exe file) the embedded database file is located.  
Default value: *.\Database.db*

**Mileage/WebServiceAddress**  
Where the HTTP api is located.  
Example value: *http://web.mydomain.com/*

#Server settings

**Mileage/Addresses**  
Where the HTTP api will be located.  
You can specify multiple addresses by separating them with a **|** character.  
Example value: *http://localhost|http://web.mydomain.com*

**Mileage/EnableDebugRequestResponseLogging**  
Whether requests and responses will be logged.  
This setting should only be set to *true* if we need to debug issues.
Default value: *false*

**Mileage/EnableDefaultMetrics**  
Whether default metrics are included in the application metrics.  
Default value: *true*  

**Mileage/EnableRavenHttpServer**  
Whether the embedded RavenDB database is available trough its HTTP server.  
Default value: *false*  

**Mileage/RavenName**  
The name for the RavenDB database and filesystem.  
Default value: *Mileage*

**Mileage/RavenHttpServerPort**  
The port where the HTTP server of the embedded RavenDB database is available.  
Default value: *8000*

**Mileage/LicensePath**  
Where (relative to the application .exe file) the license file is located.  
Example value: *.\license.lic*

**Mileage/CompressResponses**  
Whether responses should be compressed.  
Default value: *true*

**Mileage/FormatResponses**  
Whether responses should be formatted.  
Default value: *false*
