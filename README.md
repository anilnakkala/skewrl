skewrl
======

Skewrl Reference application. Skewrl implements an open source URL shortener using C#/.NET/Windows Azure

Skewrl uses Windows Azure SDK 1.8
Google Chart API
Unity dependency framework

1) Make the following changes in the web.config file of Skewrl.Web.UI project

<!-- This is the website where URL Shortener service is hosted, for example, something similar to http://goog.gl-->
    <add key="URLShortenerHost" value="http://localhost:81/" />
    <add key="FB_AppID" value="" />
    <add key="FB_Secret" value="" />
    <add key="Twitter_Key" value="" />
    <add key="Twitter_Secret" value="" />
    <add key="MS_Key" value="" />
    <add key="MS_Secret" value="" />
	

 <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=<ur local server>;Initial Catalog=aspnet_91eb661c340f4a62afb69e2c0b415b63;Integrated Security=True;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
  </connectionStrings>
  
Please see http://www.nakkala.net/2013/02/developing-url-shortening-service-using.html for more information.