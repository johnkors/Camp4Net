<====>
Usage:

In your web.config, define the user sending the messages (UserApiKey), your site on campfirenow.com and which room on your site you want to use. If your site is at http://mylovelysite.campfirenow.com, set Campfire.Site to mylovelysite

	<appSettings >
		<add key="Campfire.UserApiKey" value="YourUserApiKey"/> 
		<add key="Campfire.Site" value="YourSite"/> 
		<add key="Campfire.Room" value="123456"/> 
	</appSettings>
  
Also, include the httpmodule as any other httpmodule:

	<httpModules>
		<add name="CampfireModule" type="Camp4Net.CampfireHttpModule" />
	</httpModules>   


