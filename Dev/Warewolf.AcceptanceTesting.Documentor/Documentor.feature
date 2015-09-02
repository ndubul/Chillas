Feature: Documentor
	In order to provide a description for each component to the user 
	As a Warewolf user
	I want to be shown all aspects of item clicked

@ignore
Scenario: Message displayed in Documentor window
	Given I click "New DataBase Service Connector"
	Then "New DB Connector" tab is opened
	And Data Source is focused
	And "1 Data Source" is "Enabled"
	When I select "DemoDB" as data source
	Then the Documentor window is updated with the message 
	| "Database Service Source Types             |
	| Allows you to select a database souce type |
