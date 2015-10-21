@EmailSource
Feature: Email Source
	In order to share settings
	I want to save my Email source Settings
	So that I can reuse them

# Ensure New Email source tab is opened when click on 'New Email source' button.
# Ensure Title is saved and displayed correctly
# Ensure From defaults to UserName
# Ensure User is able to Edit saves From and To
# Ensure system is throwing validation message when send is unsuccessful


@EmailSource
Scenario: Create New Email source
	Given I open New Email Source
	Then "New Email Source" tab is opened
	And the title is "New Email Source"
	And "Host" input is ""
	And "User Name" input is ""
	And "Password" input is ""
	And "Enable SSL" input is "False"
	And "Port" input is "25"
	And "Timeout" input is "100"
	And "From" input is ""
	And "To" input is ""
	And "Send" is "Disabled"
	And "Save" is "Disabled"

@EmailSource
Scenario: From Defaults to User Name But Not After Change
	Given I open New Email Source
	Then "New Email Source" tab is opened
	When "User Name" input is "someone"
	Then "From" input is "someone"
	When "From" input is "some"
	Then "User Name" input is "someone"

@EmailSource
Scenario: Enable Send and Enable Save With Validation
	Given I open New Email Source
	Then "New Email Source" tab is opened
	And I type Host as "smtp.gmail.com"
	And I type Username as "someone"
	And I type Password as "123456"
	And "Enable SSL" input is "False"
	And "Port" input is "25"
	And "Timeout" input is "100"
	And "Send" is "Enabled"
	And "Save" is "Disabled"
	And I type To as "another@rt.com"
	And "Send" is "Enabled"
	When I click "Send"
	And Send is "Successful"
	Then "Save" is "Enabled"
	When I save as "TestEmail"
	And the save dialog is opened
    Then the title is "TestEmail"
	And "TestEmail" tab is opened
	
@EmailSource
Scenario: Fail Send
	Given I open New Email Source
	Then "New Email Source" tab is opened
	And I type Host as "smtp.gmail.com"
	And I type Username as "someone"
	And I type Password as "123456"
	And "Enable SSL" input is "False"
	And "Port" input is "25"
	And "Timeout" input is "100"
	And "Send" is "Enabled"
	And "Save" is "Disabled"
	And I type From as "someone@somewhere.com"
	And I type To as "sd@sdfsd@fdfs.com"
	Then "Send" is "Enabled"
	And "Send" is "Unsuccessful"
	Then Send is "Failed to Send: One or more errors occurred"
	And "Save" is "Disabled"

@EmailSource
Scenario: Edit saves From and To
	Given I open "Test Email Source"
	Then "Test Email Source" tab is opened
	And "Host" input is "smtp.gmail.com"
	And "User Name" input is "someone"
	And "Password" input is "123456"
	And "Enable SSL" input is "False"
	And "Port" input is "25"
	And "Timeout" input is "100"
	And "From" input is "this@to.com"
	And "To" input is "another@rt.com"
	And "Send" is "Enabled"
	And "Save" is "Disabled"
	
