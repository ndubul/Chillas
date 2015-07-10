@CreatingNewDBSource
Feature: New Database Source
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers


##// REQUIREMENTS
##* Ensure User allows to save server source with windows credentials
##* Ensure user allows to save server source as specfic user.
##* Ensure user is able to select Authonication type as Windows or User
##* Ensure UserName and Password fields are visible when user selects authentication type as User
##* Ensure UserName and Password fields are Disappear when user changes authentication type from User to Windows.
##* Ensure user is testing the source before saving
##* Ensure user is allowing to save server source when test connection is successfull
##* Ensure user is allowing to save server source when test connection is Unsuccessfull
##* Ensure system is throwing validation message when test connection is unsuccessfull
##* Ensure save button is disabled before user clicks on test connection
##* Ensure save button is Enabled when test connection is successfull
##* Ensure user is able to cancel Test connection.
##* Ensur Cancel Tesrt button is in disabled when Test Connection button is enabled 
##* Ensur Tesrt Connection is in Enabled when Cancel Test button is Disabled 
##* Ensure Database dropdown is visible when test connection is successfull
##* Ensure user is able to select database from the database dropdown 



Scenario: Creating New DB Source General Testing
   Given I open New Database Source
   Then "New Database Source" tab is opened
   And title is "New Database Source"
   When I type Server as "RSAKLFSVR"
   Then the intellisense containts these options
   | Options         |
   | RSAKLFSVRGENDEV |
   | RSAKLFSVRSBSPDC |
   | RSAKLFSVRTFSBLD |
   | RSAKLFSVRWRWBLD |
   And type options contains
   | Options              |
   | Microsoft SQL Server |
   | MySQL                |
   And I type Select The Server as "RSAKLFSVRGENDEV"
   And type options has "Microsoft SQL Server" as the default
   And Database dropdown is "Invisible"
   And "Save" is "Disabled"
   And "Test Connection" is "Enabled"
   Then Username field is "InVisible"
   And Password field is "InVisible"
   Then Database dropdown is "InVisible"
   And "Test Connection" is "Enabled"
   And "Cancel Test" is "Disabled"
   When I click "Test Connection"
   Then "Cancel Test" is "Disabled"
   Then Test Connecton is "Successful"
   Then Database dropdown is "Visible"
   Then I select "Dev2TestingDB" as Database
   And "Save" is "Enabled"   
   When I save the source
   Then the save dialog is opened
   And I save the source as "SavedDBSource"
   Then "SavedDBSource" tab is opened
   And title is "Edit SavedDBSource"
   When I type Server as "RSA"
   Then "SavedDBSource *" tab is opened
   And title is "Edit SavedDBSource"
   

	
Scenario: Creating New DB Source as User Auth
    Given I open New Database Source
    And I type Server as "RSAKLFSVRGENDEV"
    And I Select Authentication Type as "User"
    Then Username field is "Visible"
    And Password field is "Visible"
    And "Test Connection" is "Disbled"
    And "Save" is "Disabled"
    When I type Username as "testuser"
    And I type Password as "test123"
    Then "Test Connection" is "Enabled" 
    And "Save" is "Disabled"
    Then Database dropdown is "InVisible"
    And "Test Connection" is "Enabled"
    Then Test Connecton is "Successful"
    And "Save" is "Disabled"
    And Database dropdown is "Visible"
    When I select "Dev2TestingDB" as Database
    Then "Save" is "Enabled" 
    When I save the source
    Then the save dialog is opened

Scenario: Incorrect Server Address Doesnt Allow Save Windows Auth
      Given I open New Database Source
	  And "Save" is "Disabled"
      And I type Server as "RSAKLFSVRTFSBLD"
      And Database dropdown is "Invisible"
      And "Save" is "Disabled"
      And I Select Authentication Type as "Windows"
      Then Username field is "InVisible"
      And Password field is "InVisible"
      Then Database dropdown is "InVisible"
      And "Test Connection" is "Enabled"
      When Test Connecton is "Unsuccessful"
      And the validation message as "Server not found"
      Then Database dropdown is "InVisible"
      And "Save" is "Disabled"

  
Scenario: Incorrect Server Address Doesnt Allow Save User Auth
      Given I open New Database Source
      And I type Server as "RSAKLFSVRTFSBLD"
      And Database dropdown is "Invisible"
      And "Save" is "Disabled"
      And I Select Authentication Type as "User"
      Then Username field is "Visible"
      And Password field is "Visible"
	  When I type Username as "testuser"
	  And I type Password as "test123"
      Then Database dropdown is "InVisible"
      And "Test Connection" is "Enabled"
      When Test Connecton is "Unsuccessful"
      And the validation message as "Server not found" 
	  # above message comes from mocked network layer and cannot be tested here
      Then Database dropdown is "InVisible"
      And "Save" is "Disabled"



Scenario: Testing as Windows and swaping it resets the test connection 
      Given I open New Database Source
      And "Save" is "Disabled"
      And I type Server as "RSAKLFSVRTFSBLD"
      And Database dropdown is "Invisible"
      And "Save" is "Disabled"
      And "Test Connection" is "Enabled"
      And I Select Authentication Type as "User"
      Then Username field is "Visible"
      And Password field is "Visible"
      And "Test Connection" is "Disabled"
      And "Save" is "Disabled"
      When I type Username as "testuser"
      And I type Password as "test123"
      Then "Test Connection" is "Enabled" 
      And "Save" is "Disabled"
      Then Database dropdown is "InVisible"
      Then Test Connecton is "Successful"
      And "Save" is "Disabled"
      And Database dropdown is "Visible"
      When I select "Dev2TestingDB" as Database
      And I Select Authentication Type as "Windows"
      Then "Test Connection" is "Enabled" 
      And "Save" is "Disabled"
      Then Test Connecton is "Successful"
      Then Database dropdown is "Visible"
      And "Save" is "Enabled"
      When I Select Authentication Type as "User"
      Then Username field is "Visible"
      And Password field is "Visible"
      And "Test Connection" is "Enabled"
      And "Save" is "Disabled"
	  Then Database dropdown is "InVisible"


Scenario: Editing saved DB Source Remembers Previouse Auth Selection
	Given I open "Edit Database Source - Test" 
    And Server as "RSAKLFSVRGENDEV"
    And "Save" is "Disabled"
    And "Test Connection" is "Enabled"
    And Authentication Type is selected as "User"
    And Username field is "testuser"
    And Password field is "******"
	And Database "Dev2TestingDB" is selected 
    And "Save" is "Disabled"
	When I Select Authentication Type as "Windows"
    Then "Test Connection" is "Enabled" 
    And "Save" is "Disabled"
    And Database dropdown is "InVisible"
    And "Test Connection" is "Enabled"
    And Test Connecton is "Successful"
    And "Save" is "Enabled"
    And Database dropdown is "Visible"
    And I select "Dev2TestingDB2" as Database
    And "Save" is "Enabled" 
    When I Select Authentication Type as "User"
    Then "Test Connection" is "Enabled" 
    And "Save" is "Disabled"
	And Database "Dev2TestingDB" is selected 
    And "Test Connection" is "Enabled"
    And "Save" is "Disabled"
	

Scenario: Editing saved DB Source Remembers credentials
	Given I open "Edit Database Source - Test" 
    And Server as "RSAKLFSVRGENDEV"
    And "Save" is "Disabled"
    And "Test Connection" is "Enabled"
    And Authentication Type is selected as "User"
    And Username field is "testuser"
    And Password field is "******"
    And "Test Connection" is "Enabled"
	And Database "Dev2TestingDB" is selected 
    And "Save" is "Disabled"
	When I Edit Server as "RSAKLFSVRWRWBLD"
	Then Authentication Type is selected as "User"
    Then Username is "testuser"
    And Password  is "******"
    Then "Test Connection" is "Enabled" 
    And "Save" is "Disabled"
    Then Database dropdown is "InVisible"
    And "Test Connection" is "Enabled"
    When Test Connecton is "Successful"
	Then "Save" is "Enabled"
	    
Scenario: Editing saved DB Source and canceling without saving
	Given I open "Edit Database Source - Test" 
    And Server as "RSAKLFSVRGENDEV"
    And "Save" is "Disabled"
    And "Test Connection" is "Enabled"
    And Authentication Type is selected as "User"
    And Username field is "testuser"
    And Password field is "******"
	And Database "Dev2TestingDB" is selected 
    And "Save" is "Disabled"
	When I Select Authentication Type as "Windows"
    Then "Test Connection" is "Enabled" 
    And "Save" is "Disabled"
    And Database dropdown is "InVisible"
    And "Test Connection" is "Enabled"
    And Test Connecton is "Successful"
    And "Save" is "Enabled"
    And Database dropdown is "Visible"
    And I select "Dev2TestingDB" as Database
    And "Save" is "Enabled" 
    When I Cancel the source
	And I open "Edit Database Source - Test" 
	Then underlying Authentication Type is selected as "User"
    Then underlying Username is "testuser"
    And underlying Password  is "******"
	Then "Test Connection" is "Enabled" 

Scenario: Cancel Test
   Given I open New Database Source
   When I type Server as "RSAKLFSVRGENDEV"
   Then the intellisense containts these options
   And Authentication Type is selected as "User"
   And Username field is "testuser"
   And Password field is "******"
   When I click "Test Connection"
   And I click "Cancel Test"
   Then validation message is "Test Cancelled"
   And "Test Connection" is "Enabled"
    And "Save" is "Disabled"
     