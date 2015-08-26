﻿@PluginService
Feature: PluginService
	In order to use .net dlls
	As a warewolf user
	I want to be able to create plugin services

Scenario: Opening Plugin Service Connector tab
	Given I open New Plugin Service Connector
	And "New Plugin Connector *" tab is opened
	And Select a source is focused
	And "1 Select a Source" is "Enabled"
	And "2 Select a Namespace" is "Enabled"
	And "3 Select an Action" is "Enabled" 
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Disabled"
	And "Save" is "Disabled"
    And "5 Defaults and Mapping" is "Disabled" 
	And input mappings are
	| Input | Default Value | Required Field | Empty Null |
	Then output mappings are
	| Output | Output Alias |


Scenario: Create new Plugin Source
	Given I open New Plugin Service Connector
	And "New Plugin Connector" tab is opened
	And Select a source is focused
	And all other steps are "Disabled"
	And the "New" button is clicked
	Then "New Plugin Source" isopened in another tab


Scenario: Creating Plugin Service by selecting existing source
	Given I open New Plugin Service Connector
	And "New Plugin Connector" tab is opened
	When I select "testingPluginSrc" as source
	And "2 Select a Namespace" is "Enabled"
	And "3 Select an Action" is "Disabled" 
	When I select "Unlimited Framework Plugins EmailPlugin" as namespace
	Then "Select an action" is "Enabled"
	When I select "DummySent" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When "Test" is clicked
	And Test Connection is "Successful"
	Then "5 Edit Default and Mapping Names" is "Enabled" 
	And "Save" is "Enabled"
	And input mappings are
	| Input   | Default Value | Required Field | Empty Null |
	| data    |               | Selected       | Selected   |
	Then output mappings are
	| Output | Output Alias |
	| Name   | Name         |
	When "Save" is clicked
	Then the Save Dialog is opened
	

Scenario: Opening saved Plugin Service 
	Given I open "IntegrationTestPluginNull" 
	And "Edit IntegrationTestPluginNull" tab is opened
	And "testingPluginSrc" is selected as source
	And "2 Select a Namespace" is "Enabled"
	And "3 Select an Action" is "Enabled"
	And "4 Provide Test Values" is "Enabled"
	And "5 Edit Default and Mapping Names" is "Enabled"   
	And I change the source to "PrimitivePlugintestSrc"
	Then "2 Select a namespace" is "Enabled"
	And "3 Select an Action" is "Enabled"
	And "4 Provide Test Values" is "Enabled"
	And "5 Edit Default and Mapping Names" is "Enabled" 
	And "Save" is "Disabled"
    When I select "Unlimited Framework Plugins EmailPlugin" as namespace
	Then "3 Select an action" is "Enabled" 
	And "FetchStringvalue" is selected as action
	Then "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	 | Name | value |
	When Test Connection is "Successful"
	Then "5 Default and Mapping" is "Enabled" 
	Then "Save" is "Enabled"
	And input mappings are
	| Input | Default Value | Required Field | Empty Null |
	| data  |               |                |            |
	Then output mappings are
	| Output | Output Alias |
	| Name   | Name         |
	When I save as "IntegrationTestPluginNull"
    Then the Save Dialog is opened
    Then title is "IntegrationTestPluginNull"


Scenario: Refreshing plugin source action step 
	Given I open "IntegrationTestPluginNull"
	And "Edit IntegrationTestPluginNull" tab is opened
	And "2 Select a Namespace" is "Enabled"
	And "3 Select an Action" is "Enabled"
	And "4 Provide Test Values" is "Enabled"
	And "5 Edit Default and Mapping Names" is "Enabled"   
	When "Refresh" is clicked
	Then "3 Select an action" is "Enabled" 
	And "FetchStringvalue" is selected as action
	And "4 Provide Test Values" is "Enabled" 
	Then "5 Default and Mapping" is "Disabled" 
	And "Test" is "Enabled"
	When Test Connection is "Successful"
	And "5 Edit Default and Mapping Names" is "Enabled" 
	Then "Save" is "Enabled"
	And input mappings are
	| Input | Default Value | Required Field | Empty Null |
	| data  |               | Checked        | Checked    |
	Then output mappings are
	| Output | Output Alias |
	| Name   | Name         |
	When "Save" is clicked
	When I save as "Testing IntegrationTestPluginNull"
    Then the Save Dialog is opened
    Then title is "Edit Testing IntegrationTestPluginNull"
	
	

Scenario: Plugin service GetType test
	Given I open New Plugin Service Connector
	When I select "testingPluginSrc" as source
	And "2 Select a Namespace" is "Enabled"
	When I select "Unlimited Framework Plugins EmailPlugin" as namespace
	Then "3 Select an Action" is "Enabled" 
	When I select "SampleSend" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When "Test" is clicked
	Then the Test Connection is "Unsuccessful"
	Then "Save" is "Disabled"
	And "5 Edit Default and Mapping Names" is "Disabled" 
	

Scenario Outline: Fromat exception error
	Given I open New Plugin Service Connector
	And "New Plugin Connector *" tab is opened
	When I select "<source>" as source
	And "2 Select a Namespace" is "Enabled"
	And "3 Select an Action" is "Disabled" 
	When I select "<namespace>" as namespace
	Then "Select an Action" is "Enabled"
	When I select "<action>" as action
	And "4 Provide Test Values" is "Enabled" 
	And "Test" is "Enabled"
	When "Test" is clicked
	And the Test Connection is "Unsuccessful"
	Examples: 
	| source                 | namespace                               | action        |
	| testingPluginSrc       | Unlimited Framework Plugins EmailPlugin | SampleSend    |
	| PrimitiveplugintestSrc | Dev2.PrimitiveTestDLL.TestClass         | FetchIntValue |
	
	
	
	
	
	
	

	
	
	
	
	
	
	
	
	








  




   




































