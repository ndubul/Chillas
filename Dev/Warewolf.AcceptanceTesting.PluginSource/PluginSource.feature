@PluginSource
Feature: PluginSource
	In order to create plugins
	As a Warewolf User
	I want to be able to select dlls as a source to be used

## New Plugin Source File
## New Plugin Source GAC
## Editing Plugin Source
## Change Plugin Source Assembly Input
## Refresh New Plugin Source File
## Refresh New Plugin Source GAC
## load all dependancies after filter cleared 
## Search while GAC tree view is loading


Scenario: New Plugin Source File
	Given I open New Plugin Source
	Then "New Plugin Source" tab is opened
	And title is "New Plugin Source"
	And I click "File System"
	Then local drive "C:\" is visible
	Then local drive "D:\" is visible
	When I open "C:\"
	And Assembly value is ""
	And "Save" is "Disabled"
	When I click 
	| Clicks                                   |
	| Development                              |
	| Dev                                      |
	| Binaries                                 |
	| MS Fakes                                 |
	| Microsoft.QualityTools.Testing.Fakes.dll |
	Then "Save" is "Enabled"
	And Assembly value is "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	When I change Assembly to "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dl"
	Then "Save" is "Disabled"
	When I change Assembly to "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	Then "Save" is "Enabled"
	When I save as "Testing Resource Save"
    Then the save dialog is opened
	Then title is "Testing Resource Save"
	And "Testing Resource Save" tab is opened
	

Scenario: New Plugin Source GAC
	Given I open New Plugin Source
	When I click "GAC"
	And Assembly is ""
	And "Save" is "Disabled"
	When I Search for "AuditPolicyGPMan"
	And I click "AuditPolicyGPManagedStubs.Interop, Version=6.1.0.0"
	Then Assembly value is "GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a.dll"
	And "Save" is "Enabled"
	When I save Plugin source

	 
Scenario: Editing Plugin Source
	Given I open "Test" plugin source
	Then title is "Test"
	And "GAC" is "visible"
	And Assembly value is "GAC:AuditPolicyGPManagedStubs, Version=6.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a.dll"
	And "Save" is "Disabled"
	When I click "BDATunePIA, Version=6.1.0.0"
	Then Assembly value is "GAC:BDATunePIA, Version=6.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35.dll"
	And "Save" is "Enabled"
	When I save Plugin source


Scenario: Change Plugin Source Assembly Input
	Given I open "Test File" plugin source
	Then title is "Test File"
	And "File System" is "visible"
	And "C:\" is "visible"
	And Assembly value is "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	And "Save" is "Disabled"
	When I click "Dev2.Common.dll"
	Then Assembly value is "C:\Development\Dev\Binaries\MS Fakes\Dev2.Common.dll"
	And "Save" is "Enabled"
	When I save Plugin source

Scenario: Refresh New Plugin Source File
	Given I open New Plugin Source
	When I click 
	| Clicks                                   |
	| File System                              |
	| C:\                                      |
	| Development                              |
	| Dev                                      |
	| Binaries                                 |
	| MS Fakes                                 |
	| Microsoft.QualityTools.Testing.Fakes.dll |
	Then "Save" is "Enabled"
	When I refresh the filter
	And Assembly value is "C:\Development\Dev\Binaries\MS Fakes\Microsoft.QualityTools.Testing.Fakes.dll"
	And "Microsoft.QualityTools.Testing.Fakes.dll" is selected

Scenario: Refresh New Plugin Source GAC
	Given I open New Plugin Source
	When I click "GAC"
	And I click "GAC"
	#And GAC tree is collapsed
	When I filter for "BDATunePIA"
	And "GAC:BDATunePIA, Version=6.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35.dll" is "visible"
	And GAC only has one option in the tree
	When I refresh the filter
	And GAC only has one option in the tree


@ignore

#Wolf-1001

Scenario: load all dependancies after filter cleared 
	Given I open New Plugin Source
	When I click "GAC"
	And GAC tree is loading
	And I filter for "BDATunePIA"
	And "GAC:BDATunePIA, Version=6.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35.dll" is "visible"
	And GAC only has one option in the tree
	When I clear the filter
	Then GAC has all option in the tree "visible"


Scenario: Search while GAC tree view is loading
	Given I open New Plugin Source
	When I click "GAC"
	And "Save" is "Disabled"
	When I filter for "vjslib" before the tree view is completely loaded
	And I click "vjslib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	Then Assembly value is "GAC:vjslib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	And "Save" is "Enabled"
	When I save Plugin source
	Then the save dialog is opened


Scenario: Clear filter using clear filter button
	Given I open New Plugin Source
	When I click "GAC"
	And "Save" is "Disabled"
	When I filter for "vjslib"
	And I click "Clear"
	Then "GAC" tree-view is repopulated
	And "Save" is "Disabled"
	
