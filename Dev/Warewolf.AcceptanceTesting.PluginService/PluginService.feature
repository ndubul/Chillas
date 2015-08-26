@PluginService
Feature: PluginService
	In order to use .net dlls
	As a warewolf user
	I want to be able to create plugin services

Scenario: Opening Plugin Service Connector tab
	Given I click "New Plugin Service Connector"
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
	Given I click "New Plugin Service Connector"
	And "New Plugin Service" tab is opened
	And Select a source is focused
	And all other steps are "Disabled"
	And the "New" button is clicked
	Then the "New Plugin Source" tab is opened
	And the focus is now the "New Plugin Source" tab


Scenario: Creating Plugin Service by selecting existing source
	Given I click "New Plugin Service Connector"
	And "New Plugin Service" tab is opened
	When I select "testingPluginSrc" as source
	And "2 Select a namespace" is "Enabled"
	And "3 Select an action" is "Disabled" 
	When I selece "Unlimited Framework Plugins EmailPlugin" as namespace
	Then "Select an action" is "Enabled"
	When I select "DummySent" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When "Test" is clicked
	And the test connection is "successful"
	Then "5 Edit Default and Mapping Names" is "Enabled" 
	And Save is "Enabled"
	And Inputs looks like
	| Input   | Default Value | Required Field | Empty Null |
	| data    |               | Selected       | Selected   |
	Then  Outputs looks like
	| Output | Output Alias |
	| Name   | Name         |
	When Save is clicked
	Then the Save Dialog is opened
	And "Save" is "Disabled"
	And "Cancel" is "Enabled"
	When a name is entered
	Then "Save" is "Enabled"
	

Scenario: Opening saved Plugin Service 
	Given I click "Edit Plugin Service - IntegrationTestPluginNull" 
	And "Edit Plugin Service - IntegrationTestPluginNull" tab is opened
	And "testingPluginSrc" is selected as source
	And "2 Select a namespace" is "Enabled"
	And "3 Select an action" is "Enabled"
	And "4 Provide Test Values" is "Enabled"
	And "5 Edit Default and Mapping Names" is "Enabled"   
	And I change the source from "testingPluginSrc" to "PrimitivePlugintestSrc"
	Then "2 Select a namespace" is "Enabled"
	And "3 Select an action" is "Disabled"
	And "4 Provide Test Values" is "Disabled"
	And "5 Edit Default and Mapping Names" is "Enabled" 
	And "Save" is "Disabled"
    When I seleced "Dev2.PrimitiveTestDLL.TestClass" as namespace
	Then "3 Select an action" is "Enabled" 
	And "FetchStringvalue" is selected as action
	Then "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	 | Name | value |
	 |      |       |
	When Test connection is "Successful"
	Then "5 Default and Mapping" is "Enabled" 
	Then Save is "Enabled"
	And Inputs looks like
	| Input | Default Value | Required Field | Empty Null |
	| data  |               |                |            |
	Then  Outputs looks like
	| Output | Output Alias |
	| Name   | Name         |
	When I save as "IntegrationTestPluginNull Save"
    Then the save dialog is opened
    Then title is "IntegrationTestPluginNull Save"


Scenario: Refreshing plugin source action step 
	Given I click "Edit Plugin Service - IntegrationTestPluginNull"
	And "Edit Plugin Service - IntegrationTestPluginNull" tab is opened
	And "2 Select a namespace" is "Enabled"
	And "3 Select an action" is "Enabled"
	And "4 Provide Test Values" is "Enabled"
	And "5 Edit Default and Mapping Names" is "Enabled"   
	When I click "Refresh" 
	Then "3 Select an action" is "Enabled" 
	And "FetchStringvalue" is selected as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When test connection is "Successful"
	And "5 Edit Default and Mapping Names" is "Enabled" 
	Then Save is "Enabled"
	And Inputs looks like
	| Input | Default Value | Required Field | Empty Null |
	| data  |               | Checked        | Checked    |
	Then  Outputs looks like
	| Output | Output Alias |
	| Name   | Name         |
	When Save is clicked
	When I save as "Testing IntegrationTestPluginNull Save"
    Then the save dialog is opened
    Then the tab is "Edit IntegrationTestPluginNull Resource Save"
	And "Testing IntegrationTestPluginNull Save" tab is opened
	
	

Scenario: Plugin service GetType test
	Given I click "New Plugin Service Connector"
	When I select "Email Plugin" as source
	And "2 Select a namespace" is "Enabled"
	When I selecet "Unlimited.Framework.Plugins.EmailPlugin" as namespace
	Then "3 Select an action" is "Enabled" 
	When I select "GetType" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When I click "Test"
	Then test connection is "Unsuccessful"
	And the "Test Result" has validation error "True"
	Then Save is "Disabled"
	And "5 Edit Default and Mapping Names" is "Disabled" 
	

Scenario Outline: Fromat exception error
	Given I click "New Plugin Service Connector"
	And "New Plugin Service" tab is opened
	When I select '<source>' as source
	And "2 Select a namespace" is "Enabled"
	And "3 Select an action" is "Disabled" 
	When I selece '<namespace>' as namespace
	Then "Select an action" is "Enabled"
	When I select '<action>' as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When "Test" is clicked
	And the test connection is "unsuccessful"
	And a studio error of "Unhandled exception" appears with the Excetion "'System.FormatException' occurred in mscorlib.dll but was not handled in user code"
	Examples: 
	| source                 | namespace                               | action        |
	| testingPluginSrc       | Unlimited Framework Plugins EmailPlugin | SampleSend    |
	| PrimitiveplugintestSrc | Dev2.PrimitiveTestDLL.TestClass         | FetchIntValue |
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	








  




   




































