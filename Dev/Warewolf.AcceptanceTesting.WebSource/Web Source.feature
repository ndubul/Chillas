@WebSource
Feature: Web Source
	In order to create a web source for web services
	As a Warewolf user
	I want to be able to manage web sources easily

@WebSource
Scenario: Creating New Web Source 
   Given I open New Web Source 
   Then "New Web Service Source" tab is opened
   And I type Address as "http://RSAKLFSVRTFSBLD/IntegrationTestSite"
   And I type Default Query as "/GetCountries.ashx?extension=json&prefix=a"
   Then "TestQuery" is "Visible"
   And TestQuery is "http://RSAKLFSVRTFSBLD/IntegrationTestSite/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And "Cancel Test" is "Disabled"
   And I Select Authentication Type as "Anonymous"
   And Username field is "InVisible"
   And Password field is "InVisible"
   When Test Connecton is "Successful"
   And "Save" is "Enabled"
   When I save the source
   Then the save dialog is opened
   When I click TestQuery
   Then the browser window opens with "http://RSAKLFSVRTFSBLD/IntegrationTestSite/GetCountries.ashx?extension=json&prefix=a"
	
@WebSource
Scenario: Creating New Web Source under auth type as user
   Given I open New Web Source
   And I type Address as "http://RSAKLFSVRTFSBLD/IntegrationTestSite"
   And I type Default Query as "/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And I Select Authentication Type as "User"
   And Username field is "Visible"
   And Password field is "Visible"
   And I type Username as "IntegrationTester"
   And I type Password as "I73573r0"
   When Test Connecton is "Successful"
   And "Save" is "Enabled"
   When I save the source
   Then the save dialog is opened
	
@WebSource
Scenario: Incorrect address anonymous auth type not allowing save
   Given I open New Web Source
   And I type Address as "sdfsdfd"
   And I type Default Query as "/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And I Select Authentication Type as "Anonymous"
   When Test Connecton is "UnSuccessful"
   And Validation message is thrown
   And "Save" is "Disabled"

@WebSource     
Scenario: Incorrect address user auth type is not allowing to save
   Given I open New Web Source
   And I type Address as "sdfsdfd"
   And I type Default Query as "/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And I Select Authentication Type as "User"
   And I type Username as "test"
   And I type Password as "I73573r0"
   When Test Connecton is "UnSuccessful"
   And Validation message is thrown
   And "Save" is "Disabled"

@WebSource
Scenario: Testing Auth type as Anonymous and swaping it resets the test connection 
   Given I open New Web Source
   And "Save" is "Disabled"
   And I type Address as "http://RSAKLFSVRTFSBLD/IntegrationTestSite" 
   And I type Default Query as "/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And I Select Authentication Type as "User"
   And I type Username as "test"
   And I type Password as "I73573r0"
   When Test Connecton is "Successful"
   And Validation message is Not thrown
   And "Save" is "Enabled"
   And I Select Authentication Type as "Anonymous"
   And Username field is "InVisible"
   And Password field is "InVisible"
   And "Save" is "Disabled"
   When Test Connecton is "Successful"
   And Validation message is Not thrown
   And "Save" is "Enabled"	 
   And I Select Authentication Type as "User"
   And Username field is "Visible"
   And Password field is "Visible"
   And "Save" is "Disabled" 
	 	 
@WebSource
Scenario: Editing saved Web Source 
   Given I open "Test" web source
   Then "Test" tab is opened
   And Address is "http://RSAKLFSVRTFSBLD/IntegrationTestSite"
   And Default Query is "/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And Select Authentication Type as "Anonymous"
   And Username field is "InVisible"
   And Password field is "InVisible"
   And "Save" is "Disabled"
   When I change Address to "http://RSAKLFSVRTFSBLD/IntegrationTestSite"
   And I type Default Query as "/GetCountries.ashx?extension=json&prefix=b"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And "Save" is "Disabled"
   When Test Connecton is "Successfull"
   Then "Save" is "Enabled" 
   When I save the source
 
 @WebSource
 Scenario: Editing saved Web Source auth type
   Given I open "Test" web source
   Then "Test" tab is opened
   And Address is "http://RSAKLFSVRTFSBLD/IntegrationTestSite"
   And  Default Query is "/GetCountries.ashx?extension=json&prefix=a"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And Select Authentication Type as "Anonymous"
   And Username field is "InVisible"
   And Password field is "InVisible"
   When I edit Authentication Type as "User"
   And Username field is "Visible"
   And Password field is "Visible"
   And Username field as "IntegrationTester"
   And Password field as "I73573r0"
   And "Save" is "Disabled"
   When Test Connecton is "Successfull"
   Then "Save" is "Enabled" 