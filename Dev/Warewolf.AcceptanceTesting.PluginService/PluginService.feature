@PluginService
Feature: PluginService
	In order to use .net dlls
	As a warewolf user
	I want to be able to create plugin services

Scenario: Opening Plugin Service Connector tab
	Given I click New Plugin Service Connector
	And "New Plugin Service" tab is opened
	And Select a source is focused
	And "1 Select a source" is "Enabled"
	And "2 Select a namespace" is "Disabled"
	And "3 Select an action" is "Disabled" 
	And "4 Provide Test Values" is "Disabled" 
	And "Test" is "Disabled"
	And "Save" is "Disabled"
    And "5 Edit Default and Mapping Names" is "Disabled" 
	And inputs are
	| Input | Default Value | Required Field | Empty Null |
	|       |               |                |            |
	Then outputs are
	| Output | Output Alias |
	|        |              |


Scenario: Create new Plugin Source
	Given I have clicked New Plugin Service Connector
	And the "Plugin Service" tab is opened
	And Select a source is focused
	And all other steps are "Disabled"
	And the "New" button is clicked
	Then the "New Plugin Source" tab is opened
	And the focus is now the "New Plugin Source" tab


Scenario: Creating Plugin Service by selecting existing source
    Given I have "New Plugin Service" tab opened
	When I select "testingPluginSrc" as source
	And "2 Select a namespace" is "Enabled"
	And "3 Select an action" is "Disabled" 
	When I selece "Unlimited Framework Plugins EmailPlugin" as namespace
	Then "Select an action" is "Enabled"
	When I select "DummySent" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When "Test" is clicked
	And the test connection is "successfull"
	Then "5 Edit Dfault and Mapping Names" is "Enabled" 
	And Save is "Enabled"
	And Inputs looks like
	| Input   | Default Value | Required Field | Empty Null |
	| data    |               | Selected       | Selected   |
	Then  Outputs looks like
	| Output | Output Alias |
	| Name   | Name         |
	When Save is clicked
	Then the Save Dialog is opened
	And "Ok" is "Disabled"
	And "cancel" is "Enabled"
	When a name is entered
	Then "Ok" is "Enabled"
	

Scenario: Opening saved Plugin Service 
	Given I have "Edit Plugin Service - IntegrationTestPluginNull" tab opened
	And "testingPluginSrc" is selected as source
	And all steps are enabled 
	And I change the source from "testingPluginSrc" to "PrimitivePlugintestSrc"
	Then all other steps become "Disabled"
	And "Save" is "Disabled"
	But "2 Select a namespace" becomes "Enabled"
    When I seleced "Dev2.PrimitiveTestDLL.TestClass" as namespace
	Then "3 Select an action" is "Enabled" 
	And "FetchStringvalue" is selected as action
	Then "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	 | Name | value |
	 |      |       |
	When Test connection is selected
	Then "5 Edit Dfault and Mapping Names" is "Enabled" 
	Then Save is "Enabled"
	And Inputs looks like
	| Input | Default Value | Required Field | Empty Null |
	| data  |               | Selected       | Selected   |
	Then  Outputs looks like
	| Output | Output Alias |
	| Name   | Name         |
	When Save is clicked
	The the Save Dialog is opened	


Scenario: Refreshing plugin source action step 
	Given I have "Edit Plugin Service - IntegrationTestPluginNull" tab opened
	And "testingPluginSrc" is selected as source
	And "Edit" button is "Enabled"
	And all steps are "Enabled"
	When "Refresh" is seleced
	Then "Select an action" is "loaded"
	And action is selected as "DummySent" as default
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When test connection is selected
	And "5 Edit Dfault and Mapping Names" is "Enabled" 
	Then Save is "Enabled"
	And Inputs looks like
	| Input | Default Value | Required Field | Empty Null |
	| data  |               | Checked        | Checked    |
	Then  Outputs looks like
	| Output | Output Alias |
	| Name   | Name         |
	When Save is clicked
	Then the Save Dialog is opened	
	When the Plugin is saved the save window is closed
	And the tab header changes to the name of the Plugin Connectors name
	
	

Scenario: Plugin service is not saving when test is unsuccesfull
    Given I have "New Plugin Service Connector" tab opened
	When I select "Email Plugin" as source
	And "2 Select a namespace" is "Enabled"
	When I selecet "Unlimited.Framework.Plugins.EmailPlugin" as namespace
	Then "3 Select an action" is "Enabled" 
	When I select "GetType" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	| host | port | from | to |
	| test | 23   | 21   | 21 |
	When test connection is "Unsuccessfull"
	Then the "Test Result" has validation error "True"
	Then Save is "Disabled"
	And "5 Edit Dfault and Mapping Names" is "Enabled" 
	And Inputs looks like
	| Input   | Default Value | Required Field | Empty Null |
	Then  Outputs looks like
	| Output | Output Alias |
	
	
	
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	








  




   




































