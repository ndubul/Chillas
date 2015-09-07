Feature: Sharepoint
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@ignore
Scenario: Creating New Sharepoint Source 
   Given I open New Sharepoint Source 
   Then "New Sharepoint Service Source" tab is opened
   And title is "New Sharepoint Service Source"
   And I type Address as "http://rsaklfsvrsharep"
   Then "New Sharepoint Service Source *" tab is opened
   And "Save" is "Disabled"
   And "Test" is "Enabled"
   And I Select Authentication Type as "Windows"
   And Username field is "InVisible"
   And Password field is "InVisible"
   When Test Connecton is "Successful"
   And "Save" is "Enabled"
   When I save as "Testing Sharepoint Resource Save"
   Then the save dialog is opened
   Then title is "Testing Sharepoint Resource Save"
   And "Testing Sharepoint Resource Save" tab is opened


Scenario: Creating New Sharepoint Source under auth type as user
   Given I open New Sharepoint Source
   And I type Address as "http://rsaklfsvrsharep"
   And "Save" is "Disabled"
   And "Test" is "Enabled"
   And I Select Authentication Type as "User"
   And Username field is "Visible"
   And Password field is "Visible"
   And I type Username as "IntegrationTester"
   And I type Password as "I73573r0"
   When Test Connecton is "Successful"
   And "Save" is "Enabled"
   When I save the source
   Then the save dialog is opened

Scenario: Incorrect address anonymous auth type not allowing save
   Given I open New Sharepoint Source
   And I type Address as "sdfsdfd"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And I Select Authentication Type as "Anonymous"
   When Test Connecton is "UnSuccessful"
   And Validation message is thrown
   And "Save" is "Disabled"

Scenario: Incorrect address user auth type is not allowing to save
   Given I open New Sharepoint Source
   And I type Address as "sdfsdfd"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And I Select Authentication Type as "User"
   And I type Username as "test"
   And I type Password as "I73573r0"
   When Test Connecton is "UnSuccessful"
   And Validation message is thrown
   And "Save" is "Disabled"

Scenario: Testing Auth type as Anonymous and swaping it resets the test connection 
   Given I open New Sharepoint Source
   And "Save" is "Disabled"
   And I type Address as "http://rsaklfsvrsharep" 
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

Scenario: Editing saved Sharepoint Source 
   Given I open "Test" Sharepoint source
   Then "Test" tab is opened
   And title is "Test"
   And Address is "http://rsaklfsvrsharep"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And Select Authentication Type as "Anonymous"
   And Username field is "InVisible"
   And Password field is "InVisible"
   And "Save" is "Disabled"
   When I change Address to "http://rsaklfsvrshareps"
   Then "Test *" tab is opened
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   And "Save" is "Disabled"
   When Test Connecton is "Successfull"
   Then "Save" is "Enabled" 
   When I save the source

 Scenario: Editing saved Sharepoint Source auth type
   Given I open "Test" Sharepoint source
   Then "Test" tab is opened
   And Address is "http://rsaklfsvrsharep"
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

Scenario: Cancel Test
   Given I open New Sharepoint Source 
   Then "New Sharepoint Source" tab is opened
   And title is "New Sharepoint Source"
   And I type Address as "http://rsaklfsvrsharep"
   When Test Connecton is "Long Running"
   And I Cancel the Test
   Then "Cancel Test" is "Disabled"
   And Validation message is thrown
   And Validation message is "Test Cancelled"