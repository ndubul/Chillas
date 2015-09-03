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
Scenario: New Email source
	Given I click "New Email Source"
	Then "New Email Source" tab is opened
	And the title is "New Email Source"
	Then "Host" has the focus
	And "Host" is ""
	And "User Name" is ""
	And "Password" is ""
	And "Enable SSL" is "No"
	And "Port" is "25"
	And "Timeout" is "100"
	And "From" is ""
	And "To" is ""
	And "Send" is "disabled"
	And "Save" is "disabled"

Scenario: From Defaults to User Name But Not After Change
	Given I click "New Email Source"
	Then "New Email Source" tab is opened
	When "User Name" is "someone"
	Then "From" is "someone"
	When "From" is "some"
	Then "User Name" is "someone"

Scenario: Enable Send and Enable Save With Validation
	Given I click "New Email Source"
	Then "New Email Source" tab is opened
	When "Host" is "smtp.gmail.com"
	And "User Name" is "someone"
	And "Password" is "•••••••"
	And "Enable SSL" is "Yes"
	And "Port" is "25"
	And "Timeout" is "100"
	And "Send" is "disabled"
	And "Save" is "disabled"
	When "To" is "noone@nowhere"
	And "Send" is "Enabled"
	When I click "Send"
	And Send validation is "Successfully Sent"
	Then "Save" is "Enabled"
	When Save is clicked
	And the save dialog is opened
	And I save as "TestEmail"
    Then the title is "TestEmail"
	And "TestEmail" tab is opened
	

Scenario: Fail Send
	Given I click "New Email Source"
	Then "New Email Source" tab is opened
	When "Host" is "smtp.gmail.com"
	And "User Name" is "someone"
	And "Password" is ""•••••••""
	And "Enable SSL" is "Yes"
	And "Port" is "25"
	And "Timeout" is "100"
	And "Send" is "disabled"
	And "Save" is "disabled"
	And "From" is "someone@somewhere.com"
	And "To" is "sd@sdfsd@fdfs.com"
	Then "Send" is "Enabled"
	And I click "Send"
	And "Send" is "Unsuccessful"
	Then Send Validation is "Failed to Send: One or more errors occurred"
	And "Save" id "disabled"


Scenario: Edit saves From and To
	Given I click "Test Email Source"
	Then "Test Email Source" tab is opened
	When the page loads
	Then "Host" has the focus
	And "Host" is "somehost"
	And "User Name" is "username"
	And "Password" is "•••••••"
	And "Enable SSL" is "No"
	And "Port" is "25"
	And "Timeout" is "100"
	And "From" is "this@to.com"
	And "To" is "another@rt.com"
	And "Send" is "enabled"
	And "Save" is "disabled"
	
