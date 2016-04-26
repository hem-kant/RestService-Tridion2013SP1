# RestService-Tridion2013SP1

DD4T RestService WebApi Tridion 2013SP1
Install:
  Download files and build the project
  Add Tridion needed configuration and jar files to the bin folder.
  Update the config files and licenses file path 
  Update the storage config file
  
Example

To request a page from the Tridion broker.

http://domainname/Tridion2013SP1Provider.svc/GetPagebyUrl/{publicationId}/{format}?url={url}
http://domainname/Tridion2013SP1Provider.svc/GetPagebyUrl/2073/xml?url=/Collection/index.aspx
