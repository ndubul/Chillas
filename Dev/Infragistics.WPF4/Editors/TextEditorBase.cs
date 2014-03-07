using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using Infragistics.Shared;
using Infragistics.Windows;
using Infragistics.Windows.Resizing;
using Infragistics.Windows.Controls;
using Infragistics.Windows.Themes;
using Infragistics.Windows.Editors.Events;
using Infragistics.Windows.Helpers;
using System.Windows.Automation.Peers;
using Infragistics.Windows.Automation.Peers.Editors;

namespace Infragistics.Windows.Editors
{


#region Infragistics Source Cleanup (Region)








#endregion // Infragistics Source Cleanup (Region)

	/// <summary>
	/// Abstract base class for text based value editors.
	/// </summary>
	/// <remarks>
	/// <para class="body">
	/// <b>TextEditorBase</b> class is an abstract base class from which text based
	/// value editors derive. For example, <see cref="XamMaskedEditor"/> and 
	/// <see cref="XamTextEditor"/> derive from this class. Note that it's not
	/// necessary for a text based editor to derive from this class however this
	/// class provides a common set of functionality for text based value editors.
	/// </para>
	/// </remarks>

	public abstract class TextEditorBase : ValueEditor
	{
        #region Constructor
        static TextEditorBase()
        {
            // AS 2/5/09 TFS13569
            ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(TextEditorBase), new FrameworkPropertyMetadata(null, new CoerceValueCallback(CoerceToolTipIsEnabled)));
        }

        /// <summary>
        /// Initializes a new <see cref="TextEditorBase"/>
        /// </summary>
        protected TextEditorBase()
        {
        } 
        #endregion //Constructor

		#region Base Overrides

		#region DoInitialization

		/// <summary>
		/// Called from OnInitialized to provide the derived classes an opportunity to 
		/// perform appropriate initialization tasks. OnInitialized implementation enters
		/// the editor into edit mode at the end if AlwaysInEditMode is true. This method 
		/// is called before that.
		/// </summary>
		protected override void DoInitialization( )
		{
			base.DoInitialization( );

			this.SyncDisplayText( );
		}

		#endregion // DoInitialization

		#region OnPropertyChanged

		/// <summary>
		/// Called when a property value has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPropertyChanged( DependencyPropertyChangedEventArgs e )
		{
			base.OnPropertyChanged( e );

			DependencyProperty dp = e.Property;

			// SSP 2/6/09 TFS10470
			// When Format or FormatProvider changes, re-format the display text based on the new values.
			// 
			if ( LanguageProperty == dp || FormatProperty == dp || FormatProviderProperty == dp )
			{
				this.SyncDisplayText( );
			}
			// SSP 3/3/09 TFS13675
			// 
			else if ( NullTextProperty == dp )
			{
				this.SyncDisplayText( );
			}
		} 

		#endregion // OnPropertyChanged

		#region SyncValuePropertiesOverride

		
		
		

		
#region Infragistics Source Cleanup (Region)












#endregion // Infragistics Source Cleanup (Region)


		/// <summary>
		/// Overridden. Called to synchronize value and text properties. Derived classes can override this
		/// method if they have their own value properties (like the XamCheckEditor which has
		/// IsChecked property) because the Value, Text and all other editor specific value
		/// related properties need to be kept in sync. Default implementation synchronizes
		/// Value and Text properties.
		/// </summary>
		/// <param name="prop">Property that changed.</param>
		/// <param name="newValue">New value of the property.</param>
		/// <param name="error">Set this to the any error message if synchronization fails (for example
		/// because of type conversion or some other problem with syncrhonizing the new value).</param>
		/// <returns>Value indicating whether the new value should be considered valid. If false is
		/// returned, IsValueValid property will be set to false.</returns>
		internal override bool SyncValuePropertiesOverride( DependencyProperty prop, object newValue, out Exception error )
		{
			bool retVal = base.SyncValuePropertiesOverride( prop, newValue, out error );
			this.SyncDisplayText( );
			return retVal;
		}

		#endregion // SyncValuePropertiesOverride

		#endregion // Base Overrides

		#region Protected Properties

		#region DefaultValueToDisplayTextConverter

		/// <summary>
		/// Returns the default converter used for converting between the value and the text.
		/// </summary>
		/// <para class="body">
		/// DefaultValueToDisplayTextConverter returns a value converter that provides the default
		/// logic for converting between value and display text. Derived editor classes can override
		/// this property to return editor specific conversion logic. If you want to provide
		/// custom conversion logic, use the <see cref="ValueEditor.ValueToTextConverter"/>
		/// and <see cref="TextEditorBase.ValueToDisplayTextConverter"/> properties.
		/// </para>
		protected virtual IValueConverter DefaultValueToDisplayTextConverter
		{
			get
			{
				return ValueEditorDefaultConverter.ValueToDisplayTextConverter;
			}
		}

		#endregion // DefaultValueToDisplayTextConverter

		#endregion // Protected Properties

		#region Public Properties

		#region DisplayText

		
		
		
		/// <summary>
		/// DisplayText property key.
		/// </summary>
		protected static readonly DependencyPropertyKey DisplayTextPropertyKey = DependencyProperty.RegisterReadOnly(
			"DisplayText",
			typeof( string ),
			typeof( TextEditorBase ),
			new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.None,
				null )
			);

		/// <summary>
		/// Identifies the Read-Only <see cref="DisplayText"/> dependency property
		/// </summary>
		public static readonly DependencyProperty DisplayTextProperty = DisplayTextPropertyKey.DependencyProperty;

		/// <summary>
		/// Returns the display text. Note that display text is only used when the editor is not in edit mode.
		/// </summary>
		//[Description( "Returns the display text that's used when the editor is not in edit mode." )]
		//[Category( "Behavior" )]
		[Bindable( true )]
		[Browsable( false )]
		[ReadOnly( true )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public string DisplayText
		{
			get
			{
				return (string)this.GetValue( DisplayTextProperty );
			}
		}

		#endregion // DisplayText

		#region NullText

		/// <summary>
		/// Identifies the <see cref="NullText"/> dependency property
		/// </summary>
		public static readonly DependencyProperty NullTextProperty = DependencyProperty.Register(
			"NullText",
			typeof( string ),
			typeof( TextEditorBase ),
			new FrameworkPropertyMetadata( string.Empty, FrameworkPropertyMetadataOptions.None )
			);

		/// <summary>
		/// The text to display when the value of the editor is null and the editor is not in edit mode.
		/// The default value is empty string.
		/// </summary>
		/// <remarks>
		/// <para class="body">
		/// Note that the <b>NullText</b> does not specify whether the user is allowed to enter null values
		/// into the editor. For that, use the <see cref="ValueEditor.IsNullable"/> property.
		/// </para>
		/// </remarks>
		//[Description( "The text to display when the value is null and the editor is not in edit mode." )]
		//[Category( "Behavior" )]
		[Bindable( true )]
		public string NullText
		{
			get
			{
				return (string)this.GetValue( NullTextProperty );
			}
			set
			{
				this.SetValue( NullTextProperty, value );
			}
		}

		/// <summary>
		/// Returns true if the NullText property is set to a non-default value.
		/// </summary>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public bool ShouldSerializeNullText( )
		{
			return Utilities.ShouldSerialize( NullTextProperty, this );
		}

		/// <summary>
		/// Resets the NullText property to its default state.
		/// </summary>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public void ResetNullText( )
		{
			this.ClearValue( NullTextProperty );
		}

		#endregion // NullText

		#region ValueToDisplayTextConverter

		/// <summary>
		/// Identifies the <see cref="ValueToDisplayTextConverter"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty ValueToDisplayTextConverterProperty = DependencyProperty.Register(
			"ValueToDisplayTextConverter",
			typeof( IValueConverter ),
			typeof( TextEditorBase ),
			new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.None )
			);

		/// <summary>
		/// Specifies the converter used for converting between display text and value.
		/// </summary>
		/// <remarks>
		/// <para class="body">
		/// The conversions between the 'string' and the <b>ValueType</b> by default are done using 
		/// built in conversion logic. You can override the default conversion logic by setting the
		/// <b>ValueToDisplayTextConverter</b> and <see cref="ValueEditor.ValueToTextConverter"/>. 
		/// <b>ValueToTextConverter</b> is used when the editor is in edit mode where as 
		/// <b>ValueToDisplayTextConverter</b> is used when the editor is not in edit mode.
		/// </para>
		/// <para class="body">
		/// Note: An editor can edit values of types other than 'string'. For example, a <i>XamTextEditor</i> 
		/// can edit values of types <i>DateTime</i>. You can specify the type of values being edited 
		/// by the editor using the <see cref="ValueType"/> property.
		/// </para>
		/// <para class="body">
		/// For most situations the default conversion logic along with the <see cref="ValueEditor.FormatProvider"/>
		/// and <see cref="ValueEditor.Format"/> format properties should be sufficient in providing various
		/// formatting capabilities.
		/// </para>
		/// <para class="body">
		/// Although the built-in default conversion logic should be sufficient for
		/// most situations, you may want make use of this functionality to provide
		/// custom logic for converting value into display text. Examples where this
		/// would be needed are if you want to present a value like tomorrow and
		/// yesterday's dates as date as words 'Tomorrow' and 'Yesterday' respectively,
		/// or apply any kind of custom formatting that could not be specified using
		/// <see cref="ValueEditor.FormatProvider"/> and <see
		/// cref="ValueEditor.Format"/> property settings.
		/// </para>
		/// <seealso cref="ValueEditor.ValueToTextConverter"/>
		/// <seealso cref="ValueEditor.FormatProvider"/>
		/// <seealso cref="ValueEditor.Format"/>
		/// <seealso cref="ValueEditor.ValueType"/>
		/// <seealso cref="ValueEditor.Value"/>
		/// <seealso cref="ValueEditor.Text"/>
		/// <seealso cref="TextEditorBase.DisplayText"/>
		/// </remarks>
		//[Description( "Specifies the converter for converting between display text and value" )]
		//[Category( "Data" )]
		public IValueConverter ValueToDisplayTextConverter
		{
			get
			{
				return (IValueConverter)this.GetValue( ValueToDisplayTextConverterProperty );
			}
			set
			{
				this.SetValue( ValueToDisplayTextConverterProperty, value );
			}
		}


		#endregion // ValueToDisplayTextConverter

		#endregion // Public Properties

		#region Public Methods

		#region ConvertDisplayTextToValue

		// SSP 5/5/09 - Clipboard Support
		// Added ConvertDisplayTextToValue to be used by the clipboard functionality in the data presenter
		// for converting text being pasted into value.
		// 
		/// <summary>
		/// Converts the specified display text to the value type using the <see cref="ValueToDisplayTextConverterResolved"/>.
		/// </summary>
        /// <param name="displayText">The display text to convert.</param>
		/// <param name="value">This out parameter will be set to the converted value.</param>
		/// <param name="error">If conversion fails, error is set to a value that indicates the error.</param>
		/// <returns>True if conversion succeeds, False otherwise.</returns>
		/// <remarks>
		/// <p class="body">
		/// ConvertDisplayTextToValue is used to convert display text into an object of type specified by 
		/// <see cref="ValueEditor.ValueType"/> property. This method is typically not used by the editor
		/// itself, however other controls utilizing the editor can call this method to convert display text
		/// into the value that gets returned from the <see cref="ValueEditor.Value"/> property. Value 
		/// property returns objects of type specified by ValueType property.
		/// </p>
		/// <p class="body">
		/// <b>NOTE:</b> This method will only succeed if <see cref="ValueToDisplayTextConverter"/> has been
		/// specified to a converter that can successfully parse the display text into the value. If 
		/// <i>ValueToDisplayTextConverter</i> has not been specified, then the framework parsing methods
		/// will be used in which case the success of this method depends on whether these methods can correctly
		/// parse the display text.
		/// </p>
		/// <p class="body">
		/// As an example of parsing display text, if the ValueType property of a <see cref="XamTextEditor"/> is set 
		/// to DateTime type, and the display text parameter is specified as "01/01/07", this method will return 
		/// the DateTime object that represents that date. Conversion will be done based on the editor's
		/// FormatProvider property and if its not specified then the current language or culture setting.
		/// </p>
		/// <p class="body">
		/// <b>Note:</b> Typically there is no need for you to call this method directly. This method is meant
		/// to be used to perform necessary conversions between display text and value by external controls like 
		/// the data presenter, which for example uses this to convert texts being pasted into cells into cell
		/// values.
		/// </p>
		/// <p class="body">
		/// <b>Note:</b> If you want to override the default conversion logic for converting between text 
		/// and value, set the <see cref="ValueEditor.ValueToTextConverter"/> and 
		/// <see cref="TextEditorBase.ValueToDisplayTextConverter"/> properties.
		/// </p>
		/// </remarks>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public bool ConvertDisplayTextToValue( string displayText, out object value, out Exception error )
		{
			error = null;
			value = null;

			try
			{
				value = this.ValueToDisplayTextConverterResolved.ConvertBack( displayText, this.ValueType,
					this, this.FormatProviderResolved as System.Globalization.CultureInfo );

				if ( null == value )
				{
					if ( null == displayText || displayText.Length == 0
						// SSP 1/13/12 TFS99243
						|| displayText == this.NullText
						)
					{
						// SSP 1/13/12 TFS99243
						// We should always use null otherwise binding will not work as it cannot convert
						// DBNull to a DateTime? value of null for example.
						// 
						//value = DBNull.Value;
						value = null;
					}
					else
					{
						error = Utils.GetTextToValueConversionError( this.ValueType, displayText );
						return false;
					}
				}
			}
			catch ( Exception e )
			{
				error = e;
				return false;
			}

			return true;
		}

		#endregion // ConvertDisplayTextToValue

		#region ConvertValueToDisplayText

		/// <summary>
		/// Converts the specified value to display text using the <b>ValueToDisplayTextConverterResolved</b>.
		/// Returns true if the conversion succeeds. This method is used to display the value of the editor
		/// when the editor is not in edit mode.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="text">This will be set to the converted text.</param>
		/// <param name="error">If conversion fails, error is set to a value that indicates the error.</param>
		/// <returns>True if success, false otherwise.</returns>
		/// <remarks>
		/// <p class="body">
		/// See remarks section of <see cref="ValueEditor.ConvertValueToText"/> method for more information.
		/// </p>
		/// </remarks>
		[EditorBrowsable( EditorBrowsableState.Advanced )]
		public bool ConvertValueToDisplayText( object value, out string text, out Exception error )
		{
			error = null;
			text = null;

			try
			{
				text = (string)this.ValueToDisplayTextConverterResolved.Convert( value, typeof( string ),
					this, this.FormatProviderResolved as System.Globalization.CultureInfo );
			}
			catch ( Exception e )
			{
				error = e;
				return false;
			}

			return true;
		}

		#endregion // ConvertValueToDisplayText

		#endregion // Public Methods

		#region Private/Internal Properties

        
#region Infragistics Source Cleanup (Region)




















#endregion // Infragistics Source Cleanup (Region)


		#region ValueToDisplayTextConverterResolved

		// SSP 2/4/09 - NAS9.1 Record Filtering
		// Made ValueToDisplayTextConverterResolved public so the data presenter can convert 
		// cell values to text for displaying in filter drop-down.
		// 
		/// <summary>
		/// Resolved converter used for converting editor value to editor display text.
		/// </summary>
		/// <seealso cref="ValueEditor.ValueToTextConverter"/>
		/// <seealso cref="ValueToDisplayTextConverter"/>
		[ Browsable( false ), EditorBrowsable( EditorBrowsableState.Advanced ) ]
		public IValueConverter ValueToDisplayTextConverterResolved
		{
			get
			{
				IValueConverter converter = this.ValueToDisplayTextConverter;
				if ( null == converter )
					converter = this.DefaultValueToDisplayTextConverter;

				return converter;
			}
		}

		#endregion // ValueToDisplayTextConverterResolved

		#endregion // Private/Internal Properties

		#region Private/Internal Methods

        #region CoerceToolTipIsEnabled
        private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
        {
            // AS 2/5/09 TFS13569
            // Coerce the ToolTipService.IsEnabled to false while dropped down 
            // just as the intrinsic combobox and menu item do.
            //
            TextEditorBase editor = (TextEditorBase)d;

            if (editor.HasOpenDropDown)
                return KnownBoxes.FalseBox;

            return value;
        }
        #endregion //CoerceToolTipIsEnabled

        #region MouseLeftButtonDown_ToggleDropDownHelper

		// AS 9/3/08 NA 2008 Vol 2
        // Moved here from XamComboEditor
        //
        /// <summary>
        /// This is called to drop down the drop-down when the editor is entered into edit mode
        /// as a result of a mouse click. When the editor enters edit mode, its template cannot
        /// toggle the drop down because the template is switched from edit to render template.
        /// This interferes with framework mouse processing and prevents the drop-down buttons to
        /// from receiving clicks. Therefore we have to do it manually. This method is for that.
        /// </summary>
        /// <param name="e"></param>
        internal void MouseLeftButtonDown_ToggleDropDownHelper(MouseButtonEventArgs e)
        {
            #region Moved to helper
            
#region Infragistics Source Cleanup (Region)













































#endregion // Infragistics Source Cleanup (Region)

            #endregion //Moved to helper
            bool toggleDropDown = this.HasDropDown && this.ShouldToggleDropDown(e);

            // SSP 11/9/07
            // Try to focus the editor if it already doesn't have focus. If it fails to get focus then
            // don't toggle the drop-down.
            // 
            if (toggleDropDown && !this.IsInEditMode && !this.IsFocusWithin)
            {
                this.Focus();
                if (!this.IsFocusWithin)
                    toggleDropDown = false;
            }

            if (toggleDropDown)
            {
                this.ToggleDropDown();
                e.Handled = true;
            }
        }

        #endregion // MouseLeftButtonDown_ToggleDropDownHelper

        // AS 9/10/08 NA 2008 Vol 2
        #region RaiseDropDownClosedIfPending
        internal virtual void RaiseDropDownClosedIfPending()
        {
        }
        #endregion //RaiseDropDownClosedIfPending 

		#region SyncDisplayText

		
		
		
		/// <summary>
		/// Synchronizes the DisplayText property with the value.
		/// </summary>
		internal void SyncDisplayText( )
		{
			string displayText;
			Exception error;

			bool success = this.ConvertValueToDisplayText( this.Value, out displayText, out error );

			Debug.Assert( success, "There shouldn't be any trouble converting to text." );

			this.SetValue( DisplayTextPropertyKey, displayText );
		}

		#endregion // SyncDisplayText

        #region ShouldToggleDropDown
        // AS 9/3/08 NA 2008 Vol 2
        // Moved here from MouseLeftButtonDown_ToggleDropDownHelper
        //
        internal virtual bool ShouldToggleDropDown(MouseButtonEventArgs e)
        {
            const string DROP_DOWN_BUTTON_NAME = "DropDownButton";
            // AS 9/19/08
            // Make this a part on the appropriate classes (Combo/DateTimeEditor) but 
            // continue to look for this name for backward compatibility.
            //
            //FrameworkElement dropDownButton = this.GetTemplateChild(DROP_DOWN_BUTTON_NAME) as FrameworkElement;
            const string DROP_DOWN_BUTTON_PARTNAME = "PART_DropDownButton";
            FrameworkElement dropDownButton = (this.GetTemplateChild(DROP_DOWN_BUTTON_PARTNAME) ?? this.GetTemplateChild(DROP_DOWN_BUTTON_NAME)) as FrameworkElement;
            if (null == dropDownButton && this.IsInEditMode)
            {
                // AS 2/2/09
                //dropDownButton = Utilities.GetDescendantFromName(this, DROP_DOWN_BUTTON_NAME);
                dropDownButton = Utilities.GetDescendantFromName(this, DROP_DOWN_BUTTON_PARTNAME)
                    ?? Utilities.GetDescendantFromName(this, DROP_DOWN_BUTTON_NAME);
                if (null == dropDownButton || dropDownButton.ActualHeight <= 0 || dropDownButton.ActualWidth <= 0)
                    this.UpdateLayout();
            }

            if (null != dropDownButton && Utils.IsMouseOverElement(dropDownButton, e))
                return true;

            return false;
        } 
        #endregion //ShouldToggleDropDown

        #region ToggleDropDown
        // AS 9/3/08 NA 2008 Vol 2
        // Added virtual
        //
        internal virtual void ToggleDropDown()
        {
        } 
        #endregion //ToggleDropDown


        // AS 9/3/08 NA 2008 Vol 2
        // Moved here from XamComboEditor
        //
        /// <summary>
        /// Updates the DropDownButtonVisibility property according to the current state of the editor
        /// and DropDownButtonDisplayMode property setting.
        /// </summary>
        internal void UpdateDropDownButtonVisibility()
        {
            // AS 9/3/08 NA 2008 Vol 2
            // Added overload.
            //
            // AS 10/3/08 TFS8634
            //this.UpdateDropDownButtonVisibility(true);
            this.UpdateDropDownButtonVisibility(this.IsEditingAllowed);
        }

        // AS 9/3/08 NA 2008 Vol 2
        // Added hideButton param since the XamDateTimeEditor may need
        // to force the button to not show.
        //
        internal void UpdateDropDownButtonVisibility(bool allowButton)
        {
            bool isVisible = false;

            if (allowButton)
            {
                DropDownButtonDisplayMode displayMode = (DropDownButtonDisplayMode)this.GetValue(XamComboEditor.DropDownButtonDisplayModeProperty);

                switch (displayMode)
                {
                    case DropDownButtonDisplayMode.Always:
                        isVisible = true;
                        break;
                    case DropDownButtonDisplayMode.Focused:
                        isVisible = this.IsInEditMode || this.IsFocusWithin;
                        break;
                    case DropDownButtonDisplayMode.OnlyInEditMode:
                        isVisible = this.IsInEditMode;
                        break;
                    case DropDownButtonDisplayMode.MouseOver:
                    default:
                        isVisible = this.IsInEditMode || this.IsMouseOver;
                        break;
                }
            }

            // AS 9/3/08 NA 2008 Vol 2
            // Try to avoid excessive boxing.
            //
            //this.SetValue( DropDownButtonVisibilityPropertyKey, isVisible ? Visibility.Visible : Visibility.Collapsed );
            this.SetValue(XamComboEditor.DropDownButtonVisibilityPropertyKey, isVisible ? KnownBoxes.VisibilityVisibleBox : KnownBoxes.VisibilityCollapsedBox);
        }

		#endregion // Private/Internal Methods

        #region Base class overrides

        #region OnEditModeEnding
        /// <summary>
        /// This method is called when the control is about to exit edit mode. This method raises 
        /// <see cref="ValueEditor.EditModeEnding"/> event.
        /// </summary>
        /// <remarks>
        /// <seealso cref="ValueEditor.EditModeEnding"/>
        /// </remarks>
        protected override void OnEditModeEnding(EditModeEndingEventArgs args)
        {
            // AS 9/10/08 NA 2008 Vol 2
            // We need to make sure to close the dropdown before exiting edit mode
            //
            if (this.HasOpenDropDown)
            {
                this.ToggleDropDown();
            }

            // AS 9/10/08 NA 2008 Vol 2
            // Also since the popup's Closed event may not have been raised and we
            // are going to change the template, we need to force the closed event
            // to be fired.
            //
            this.RaiseDropDownClosedIfPending();

			// MD 7/16/10 - TFS26592
			// Call off to the new helper method to raise the event. 
            //this.RaiseEvent(args);
			this.RaiseEventHelper(args);
        }

        #endregion //OnEditModeEnding

        #region OnMouseLeftButtonDown

        // AS 9/3/08 NA 2008 Vol 2
        // Moved here from XamComboEditor
        //
        /// <summary>
        /// Called when the left mouse button is pressed.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (null == this.Host && this.Focusable && this.IsEnabled)
            {
                // When the mouse is clicked on the editor when the editor is not in edit mode, we need to enter it 
                // into edit mode and toggle its drop-down. This is something that cannot be done via xaml since 
                // the template switches from render to edit template which interferes with framework mouse processing 
                // causing the toggle buttons in xaml to not receive click event. Therefore we need to do it manually
                // when the the editor enters edit mode via click.
                // 
                if (!this.IsInEditMode)
                    this.MouseLeftButtonDown_ToggleDropDownHelper(e);
            }

            base.OnMouseLeftButtonDown(e);
        }
        #endregion // OnMouseLeftButtonDown

        #region OnPreviewMouseLeftButtonDown

        // AS 9/3/08 NA 2008 Vol 2
        // Moved here from XamComboEditor
        //
        /// <summary>
        /// Preview method called when the left mouse button is pressed.
        /// </summary>
        /// <param name="e">The event args containing more information on the mouse event.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // When the mouse is clicked on the editor when the editor is not in edit mode, we need to enter it 
            // into edit mode and toggle its drop-down. This is something that cannot be done via xaml since 
            // the template switches from render to edit template which interferes with framework mouse processing 
            // causing the toggle buttons in xaml to not receive click event. Therefore we need to do it manually
            // when the the editor enters edit mode via click.
            // 
            if (null != this.Host && this.Focusable && this.IsEnabled
                && this.Host._previewMouseLeftButtonDown_EnterEditMode_TimeStamp == e.Timestamp)
            {
                this.MouseLeftButtonDown_ToggleDropDownHelper(e);
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }
        #endregion // OnPreviewMouseLeftButtonDown

        #endregion //Base class overrides
	}
}

#region Copyright (c) 2001-2012 Infragistics, Inc. All Rights Reserved
/* ---------------------------------------------------------------------*
*                           Infragistics, Inc.                          *
*              Copyright (c) 2001-2012 All Rights reserved               *
*                                                                       *
*                                                                       *
* This file and its contents are protected by United States and         *
* International copyright laws.  Unauthorized reproduction and/or       *
* distribution of all or any portion of the code contained herein       *
* is strictly prohibited and will result in severe civil and criminal   *
* penalties.  Any violations of this copyright will be prosecuted       *
* to the fullest extent possible under law.                             *
*                                                                       *
* THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
* TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
* TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
* CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
* THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF INFRAGISTICS, INC. *
*                                                                       *
* UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
* PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
* SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY INFRAGISTICS PRODUCT.    *
*                                                                       *
* THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
* CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF INFRAGISTICS,      *
* INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
* INSURE ITS CONFIDENTIALITY.                                           *
*                                                                       *
* THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
* PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
* EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
* THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
* SOURCE CODE CONTAINED HEREIN.                                         *
*                                                                       *
* THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
* --------------------------------------------------------------------- *
*/
#endregion Copyright (c) 2001-2012 Infragistics, Inc. All Rights Reserved