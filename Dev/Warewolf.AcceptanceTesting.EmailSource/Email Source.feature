Feature: Email Source
	In order to share settings
	I want to save my Email source Settings
	So that I can reuse them

#@EmailSource
#Scenario: New Email source
#	Given a new email source
#	When the page loads
#	And the title is "New Email Source"
#	Then "Host" has the focus
#	And "Host" is ""
#	And "User Name" is ""
#	And "Password" is ""
#	And "Enable SSL" is "No"
#	And "Port" is "25"
#	And "Timeout" is "100"
#	And "From" is ""
#	And "To" is ""
#	And "Send" is "disabled"
#	And "Save" is "disabled"
#
#Scenario: From Defaults to User Name But Not After Change
#	Given a new email source
#	When "User Name" is "someone"
#	Then "From" is "someone"
#	When "From" is "some"
#	Then "User Name" is "someone"
#
#Scenario: Enable Send and Enbale Save With Validation
#	Given a new email source
#	When "Host" is "smtp.gmail.com"
#	And "User Name" is "someone"
#	And "Password" is "•••••••"
#	And "Enable SSL" is "Yes"
#	And "Port" is "25"
#	And "Timeout" is "100"
#	And "Send" is "enabled"
#	And "Save" is "disabled"
#	And "From" is "someone@somewhere"
#	And "To" is "noone@nowhere.com"
#	And "Send" is "disabled"
#	And "From" is "someone@somewhere.com"
#	And "To" is "noone@nowhere"
#	And "Send" is "disabled"
#	And "From" is "someone@somewhere.com"
#	And "To" is "noone@nowhere.com"
#	And "Send" is "enabled"
#	And I click "Send"
#	And Send validation is "Successfully Sent"
#	And "Save" is "enabled"
#	And "Host" is ""
#	And Send validation is ""
#	And "Send" is "enabled"
#	And "Save" is "disabled"
#	
#Scenario: Fail Send
#	Given a new email source
#	When "Host" is "smtp.gmail.com"
#	And "User Name" is "someone"
#	And "Password" is ""•••••••""
#	And "Enable SSL" is "Yes"
#	And "Port" is "25"
#	And "Timeout" is "100"
#	And "Send" is "enabled"
#	And "Save" is "disabled"
#	And "From" is "someone@somewhere.com"
#	And "To" is "noone@nowhere.com"
#	And I click "Send"
#	# Add system message
#	Then Send Validation is "Failed to Send: ..."
#	And "Save" id "disabled"
#
#
#Scenario: Edit saves From and To
#	Given a an existing email source
#	When the page loads
#	Then "Host" has the focus
#	And "Host" is "somehost"
#	And "User Name" is "username"
#	And "Password" is "•••••••"
#	And "Enable SSL" is "No"
#	And "Port" is "25"
#	And "Timeout" is "100"
#	And "From" is "this@to.com"
#	And "To" is "another@rt.com"
#	And "Send" is "enabled"
#	And "Save" is "disabled"
#	
