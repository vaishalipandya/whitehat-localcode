using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace WhiteHatSec.Localization.Culture.Resource
{
    /// <summary>
    ///     Culture Manager For Localization.
    /// </summary>
    [ToolboxItem(true)]
    public class CultureManager : Component
    {
        #region Static/Constant Member Variables

        /// <summary>
        ///     The anchor left right.
        /// </summary>
        private const AnchorStyles AnchorLeftRight = AnchorStyles.Left | AnchorStyles.Right;

        /// <summary>
        ///     The anchor top bottom.
        /// </summary>
        private const AnchorStyles AnchorTopBottom = AnchorStyles.Top | AnchorStyles.Bottom;

        /// <summary>
        ///     The application UI culture.
        /// </summary>
        private static CultureInfo applicationUiCulture;

        /// <summary>
        /// Synchronized culture
        /// </summary>
        private static bool synchronizeThreadCulture = true;

        #endregion

        #region Member Variables

        /// <summary>
        ///     The control being managed.
        /// </summary>
        private Control managedControl;

        /// <summary>
        ///     The current UI culture for the managed control.
        /// </summary>
        private CultureInfo uiCulture;

        /// <summary>
        ///     If true form size is preserved when changing culture.
        /// </summary>
        private bool preserveFormSize = true;

        /// <summary>
        ///     If true form location is preserved when changing culture.
        /// </summary>
        private bool preserveFormLocation = true;

        /// <summary>
        ///     Properties to be excluded when applying culture resources.
        /// </summary>
        private List<string> excludeProperties = new List<string>();

        /// <summary>
        /// Synchronized UI Culture
        /// </summary>
        private bool synchronizeUiCulture = true;

        /// <summary>
        ///     The current auto scale factor.
        /// </summary>
        private SizeF autoScaleFactor = new SizeF(1, 1);

        #endregion

        #region Public Static Methods

        /// <summary>
        ///  Represents the method that will handle the culturechanged event.      
        /// </summary>
        /// <param name="changedUiCulture">The Changed UI Culture</param>
        public delegate void CultureChangedHandler(CultureInfo changedUiCulture);

        /// <summary>
        ///     Raised when the Application UI culture is changed.
        /// </summary>
        public static event CultureChangedHandler ApplicationUiCultureChanged;

        /// <summary>
        /// Application UI culture for changing culture for whole application
        /// </summary>
        public static CultureInfo ApplicationUiCulture
        {
            get { return applicationUiCulture ?? (applicationUiCulture = Thread.CurrentThread.CurrentUICulture); }

            set
            {
                if (!value.Equals(applicationUiCulture))
                {
                    applicationUiCulture = value;
                    SetThreadUiCulture(SynchronizeThreadCulture);
                    if (ApplicationUiCultureChanged != null)
                    {
                        ApplicationUiCultureChanged(value);
                    }
                }
            }
        }

        /// <summary>
        ///     If set to true then the thread.CurrentCulture property is changed to match the current Ui culture
        /// </summary>       
        public static bool SynchronizeThreadCulture
        {
            get { return synchronizeThreadCulture; }

            set
            {
                synchronizeThreadCulture = value;
                if (value)
                {
                    SetThreadUiCulture(true);
                }
            }
        }
        /// <summary>
        /// Set Thread UI culture 
        /// </summary>
        /// <param name="synchronizeThreadCulture">The Synchronized thread culture.</param>        
        public static void SetThreadUiCulture(bool synchronizeThreadCulture)
        {
            Thread.CurrentThread.CurrentUICulture = ApplicationUiCulture;
            if (synchronizeThreadCulture)
            {
                Thread.CurrentThread.CurrentCulture = ApplicationUiCulture.IsNeutralCulture
                    ? CultureInfo.CreateSpecificCulture(ApplicationUiCulture.Name)
                    : ApplicationUiCulture;
            }
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        ///     Raised when the UI cultire is changed for this component. Also change form to update their layout following a change to the UI culture.
        /// </summary>
        public event CultureChangedHandler UiCultureChanged;

        /// <summary>
        /// Represents the method that will handle the exclude resource  event.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public delegate bool ExcludedResourceHandler(string componentName, string propertyName);

        /// <summary>
        ///     Raised for each resource to check if it should be excluded from updating.
        /// </summary>
        public event ExcludedResourceHandler ExcludeResource;

        /// <summary>
        ///     Create a new instance of the culture application.
        /// </summary>
        public CultureManager()
        {
            ApplicationUiCultureChanged += OnApplicationUiCultureChanged;
        }

        /// <summary>
        ///     Create a new instance of the component.
        /// </summary>
        /// <param name="container">The container.</param>
        public CultureManager(IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        ///     The control or form to manage the UICulture for.
        /// </summary>
        /// <value>
        ///     The managed control.
        /// </value>
        [Description("The control or form to manage the UICulture for")]
        public Control ManagedControl
        {
            get
            {
                if (managedControl == null)
                {
                    if (Site != null)
                    {
                        IDesignerHost host = Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
                        if (host != null && host.Container != null && host.Container.Components.Count > 0)
                        {
                            managedControl = host.Container.Components[0] as Control;
                        }
                    }
                }

                return managedControl;
            }

            set { managedControl = value; }
        }

        /// <summary>
        ///     Should the form size be preserved when the culture is changed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [preserve form size]; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(true)]
        [Description("Should the form size be preserved when the culture is changed")]
        public bool PreserveFormSize
        {
            get { return preserveFormSize; }
            set { preserveFormSize = value; }
        }

        /// <summary>
        ///     Should the form location be preserved when the culture is changed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [preserve form location]; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(true)]
        [Description("Should the form location be preserved when the culture is changed")]
        public bool PreserveFormLocation
        {
            get { return preserveFormLocation; }
            set { preserveFormLocation = value; }
        }

        /// <summary>
        ///     List of properties to exclude when applying culture specific resources.
        /// </summary>
        /// <value>
        ///     The exclude properties.
        /// </value>
        /// <exception cref="System.ArgumentNullException"></exception>
        [Editor(
            "System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        [Description("List of properties to exclude when applying culture specific resources")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> ExcludeProperties
        {
            get { return excludeProperties; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                excludeProperties = value;
            }
        }

        /// <summary>
        ///     Called by framework to determine whether the ExcludeProperties should be serialised.
        /// </summary>
        /// <returns>True if the style should be serialized.</returns>
        private bool ShouldSerializeExcludeProperties()
        {
            return excludeProperties.Count > 0;
        }

        /// <summary>
        ///     Called by framework to reset the ExcludeProperties.
        /// </summary>
        private void ResetExcludeProperties()
        {
            excludeProperties.Clear();
        }

        /// <summary>
        ///     The current user interface culture for the managed control
        /// </summary>
        /// <remarks>
        ///     This can be set independently of the <see cref="ApplicationUiCulture" />, allowing
        ///     you to have an application simultaneously displaying forms with different cultures.
        ///     Set the <see cref="ApplicationUiCulture" /> to change the <see cref="UiCulture" /> of
        ///     all forms in the application which have associated CultureManagers.
        /// </remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CultureInfo UiCulture
        {
            get { return uiCulture ?? (uiCulture = ApplicationUiCulture); }

            set
            {
                if (value != null)
                {
                    ChangeUiCulture(value);
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
        }

        /// <summary>
        ///     Should the
        ///     <see cref="UiCulture" /> of this form be changed.
        ///     when the
        ///     <see cref="ApplicationUiCulture" /> is changed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [synchronize UI culture]; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(true)]
        [Description("Should the UICulture of this form be changed when the ApplicationUICulture")]
        public bool SynchronizeUiCulture
        {
            get { return synchronizeUiCulture; }
            set { synchronizeUiCulture = value; }
        }

        #endregion

        #region Local Methods

        /// <summary>
        ///     Dispose of the component.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // detach from the global event handler
                ApplicationUiCultureChanged -= OnApplicationUiCultureChanged;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///     Handle a change to <see cref="ApplicationUiCulture" />
        /// </summary>
        /// <param name="newCulture">The new culture.</param>
        protected virtual void OnApplicationUiCultureChanged(CultureInfo newCulture)
        {
            if (SynchronizeUiCulture)
            {
                ChangeUiCulture(newCulture);
            }
        }

        /// <summary>
        ///     UI Culture Changed.
        /// </summary>
        /// <param name="newCulture"></param>
        protected virtual void OnUiCultureChanged(CultureInfo newCulture)
        {
            if (UiCultureChanged != null)
            {
                UiCultureChanged(newCulture);
            }
        }

        /// <summary>
        ///     Set the UI Culture for the managed form/control.
        /// </summary>
        /// <param name="culture">The culture to change to.</param>
        protected virtual void ChangeUiCulture(CultureInfo culture)
        {
            if (!culture.Equals(uiCulture))
            {
                uiCulture = culture;
                if (managedControl != null)
                {
                    managedControl.SuspendLayout();
                    foreach (Control childControl in managedControl.Controls)
                    {
                        childControl.SuspendLayout();
                    }

                    try
                    {
                        ApplyResources(managedControl.GetType(), managedControl, culture);
                        OnUiCultureChanged(culture);
                    }
                    finally
                    {
                        foreach (Control childControl in managedControl.Controls)
                        {
                            childControl.ResumeLayout();
                        }

                        managedControl.ResumeLayout();
                    }
                }
            }
        }

        /// <summary>
        ///     Load the localized resource for the given culture into a sorted list (indexed by resource name).
        /// </summary>
        /// <param name="componentResourceManager">The resource manager to load resources from</param>
        /// <param name="culture">The culture to load resources for</param>
        /// <param name="resources">The list to load the resources into.</param>
        /// <remarks>
        ///     Recursively loads the resources by loading the resources for the parent culture first.
        /// </remarks>
        private void LoadResources(ComponentResourceManager componentResourceManager, CultureInfo culture,
            SortedList<string, object> resources)
        {
            if (!culture.Equals(CultureInfo.InvariantCulture))
            {
                LoadResources(componentResourceManager, culture.Parent, resources);
            }

            ResourceSet resourceSet = componentResourceManager.GetResourceSet(culture, true, true);
            if (resourceSet != null)
            {
                foreach (DictionaryEntry entry in resourceSet)
                {
                    resources[(string)entry.Key] = entry.Value;
                }
            }
        }

        /// <summary>
        ///     Return the current auto scaling factor.
        /// </summary>
        protected SizeF AutoScaleFactor
        {
            get { return autoScaleFactor; }
        }

        /// <summary>
        ///     Set the AutoScaleDimensions for the given control.
        /// </summary>
        /// <param name="control">The control that the property is set for.</param>
        /// <param name="size">The design time dimensions for the control.</param>
        protected void SetAutoScaleDimensions(ContainerControl control, SizeF size)
        {
            SizeF contolSize = control.CurrentAutoScaleDimensions;
            autoScaleFactor = new SizeF(contolSize.Width / size.Width, contolSize.Height / size.Height);
        }

        /// <summary>
        ///     Scale a size by the current auto scaling factor.
        /// </summary>
        /// <param name="size">The size to scale</param>
        /// <returns>The scaled size.</returns>
        protected Size AutoScaleSize(Size size)
        {
            size.Width = (int)(size.Width * AutoScaleFactor.Width);
            size.Height = (int)(size.Height * AutoScaleFactor.Height);
            return size;
        }

        /// <summary>
        ///     Scale a point by the current auto scaling factor.
        /// </summary>
        /// <param name="point">The point to scale</param>
        /// <returns>The scaled size.</returns>
        protected Point AutoScalePoint(Point point)
        {
            point.X = (int)(point.X * AutoScaleFactor.Width);
            point.Y = (int)(point.Y * AutoScaleFactor.Height);
            return point;
        }

        /// <summary>
        ///     Scale a padding by the current auto scaling factor.
        /// </summary>
        /// <param name="padding">The padding to scale.</param>
        /// <returns>The scaled padding.</returns>
        protected virtual Padding AutoScalePadding(Padding padding)
        {
            SizeF factor = AutoScaleFactor;
            padding.Top = (int)(padding.Top * factor.Height);
            padding.Bottom = (int)(padding.Bottom * factor.Height);
            padding.Left = (int)(padding.Left * factor.Width);
            padding.Right = (int)(padding.Right * factor.Width);
            return padding;
        }

        /// <summary>
        ///     Set the location of a control handling docked/anchored controls appropriately.
        /// </summary>
        /// <param name="control">The control to set the location of</param>
        /// <param name="location">The new location of the control</param>
        protected virtual void SetControlLocation(Control control, Point location)
        {
            // if the control is a form and we are preserving form location then exit
            if (control is Form && PreserveFormLocation)
            {
                return;
            }

            // if dock is set then don't change the location
            if (control.Dock != DockStyle.None)
            {
                return;
            }

            location = AutoScalePoint(location);

            // if anchored to the right (but not left) then don't change x coord
            if ((control.Anchor & AnchorLeftRight) == AnchorStyles.Right)
            {
                location.X = control.Left;
            }

            // if anchored to the bottom (but not top) then don't change y coord
            if ((control.Anchor & AnchorTopBottom) == AnchorStyles.Bottom)
            {
                location.Y = control.Top;
            }

            control.Location = location;
        }

        /// <summary>
        ///     Apply a resource for an extender provider to the given control.
        /// </summary>
        /// <param name="extenderProviders">Extender providers for the parent control indexed by type</param>
        /// <param name="control">The control that the extended resource is associated with</param>
        /// <param name="propertyName">The extender provider property name</param>
        /// <param name="value">The value to apply</param>
        /// <remarks>
        ///     This can be overridden to add support for other ExtenderProviders.  The default implementation
        ///     handles <see cref="ToolTip">ToolTips</see>, <see cref="HelpProvider">HelpProviders</see>,
        ///     and <see cref="ErrorProvider">ErrorProviders</see>
        /// </remarks>
        protected virtual void ApplyExtenderResource(
            Dictionary<Type,
                IExtenderProvider> extenderProviders,
            Control control,
            string propertyName,
            object value)
        {
            IExtenderProvider extender;

            if (propertyName == "ToolTip")
            {
                if (extenderProviders.TryGetValue(typeof(ToolTip), out extender))
                {
                    (extender as ToolTip).SetToolTip(control, value as string);
                }
            }
            else if (propertyName == "HelpKeyword")
            {
                if (extenderProviders.TryGetValue(typeof(HelpProvider), out extender))
                {
                    (extender as HelpProvider).SetHelpKeyword(control, value as string);
                }
            }
            else if (propertyName == "HelpString")
            {
                if (extenderProviders.TryGetValue(typeof(HelpProvider), out extender))
                {
                    (extender as HelpProvider).SetHelpString(control, value as string);
                }
            }
            else if (propertyName == "ShowHelp")
            {
                if (extenderProviders.TryGetValue(typeof(HelpProvider), out extender))
                {
                    (extender as HelpProvider).SetShowHelp(control, (bool)value);
                }
            }
            else if (propertyName == "Error")
            {
                if (extenderProviders.TryGetValue(typeof(ErrorProvider), out extender))
                {
                    (extender as ErrorProvider).SetError(control, value as string);
                }
            }
            else if (propertyName == "IconAlignment")
            {
                if (extenderProviders.TryGetValue(typeof(ErrorProvider), out extender))
                {
                    (extender as ErrorProvider).SetIconAlignment(control, (ErrorIconAlignment)value);
                }
            }
            else if (propertyName == "IconPadding")
            {
                if (extenderProviders.TryGetValue(typeof(ErrorProvider), out extender))
                {
                    (extender as ErrorProvider).SetIconPadding(control, (int)value);
                }
            }
        }

        /// <summary>
        ///     Check whether a given component/property should be excluded.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected virtual bool IsExcluded(string componentName, string propertyName)
        {
            string resourceName = componentName + "." + propertyName;
            foreach (string value in excludeProperties)
            {
                if (value.Contains("."))
                {
                    if (resourceName.Contains(value))
                    {
                        return true;
                    }
                }
                else
                {
                    if (value == propertyName)
                    {
                        return true;
                    }
                }
            }

            if (ExcludeResource != null)
            {
                return ExcludeResource(componentName, propertyName);
            }

            return false;
        }

        /// <summary>
        ///     Recursively apply localized resources to a component and its constituent components.
        /// </summary>
        /// <param name="componentType">The type we are applying resources for</param>
        /// <param name="componentInstance">The component instance to apply resources to</param>
        /// <param name="culture">The culture resources to apply</param>
        protected virtual void ApplyResources(Type componentType, IComponent componentInstance, CultureInfo culture)
        {
            // check whether there are localizable resources for the type - if not we are done
            Stream resourceStream = componentType.Assembly.GetManifestResourceStream(componentType.FullName + ".resources");
            if (resourceStream == null)
            {
                return;
            }

            // recursively apply the resources localized in the base type
            Type parentType = componentType.BaseType;
            if (parentType != null)
            {
                ApplyResources(parentType, componentInstance, culture);
            }

            // load the resources for this IComponent type into a sorted list
            ComponentResourceManager resourceManager = new ComponentResourceManager(componentType);
            SortedList<string, object> resources = new SortedList<string, object>();
            LoadResources(resourceManager, culture, resources);

            // build a lookup table of components indexed by resource name
            Dictionary<string, IComponent> components = new Dictionary<string, IComponent>();

            // build a lookup table of extender providers indexed by type
            Dictionary<Type, IExtenderProvider> extenderProviders = new Dictionary<Type, IExtenderProvider>();


            components["$this"] = componentInstance;
            FieldInfo[] fields = componentType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                string fieldName = field.Name;

                // check whether this field is a localized component of the parent
                string resourceName = ">>" + fieldName + ".Name";
                if (resources.ContainsKey(resourceName))
                {
                    IComponent childComponent = field.GetValue(componentInstance) as IComponent;
                    if (childComponent != null)
                    {
                        components[fieldName] = childComponent;

                        // apply resources localized in the child component type
                        ApplyResources(childComponent.GetType(), childComponent, culture);

                        // if this component is an extender provider then keep track of it
                        if (childComponent is IExtenderProvider)
                        {
                            extenderProviders[childComponent.GetType()] = childComponent as IExtenderProvider;
                        }
                    }
                }
            }

            // now process the resources 
            foreach (KeyValuePair<string, object> pair in resources)
            {
                string resourceName = pair.Key;
                object resourceValue = pair.Value;
                string[] resourceNameParts = resourceName.Split('.');
                string componentName = resourceNameParts[0];
                string propertyName = resourceNameParts[1];

                if (componentName.StartsWith(">>"))
                {
                    continue;
                }

                if (IsExcluded(componentName, propertyName))
                {
                    continue;
                }

                IComponent component;
                if (!components.TryGetValue(componentName, out component)) continue;

                // some special case handling for control sizes/locations
                Control control = component as Control;
                if (control != null)
                {
                    switch (propertyName)
                    {
                        case "AutoScaleDimensions":
                            SetAutoScaleDimensions(control as ContainerControl, (SizeF)resourceValue);
                            continue;
                        case "Size":
                            continue;
                        case "Location":
                            SetControlLocation(control, (Point)resourceValue);
                            continue;
                        case "Padding":
                        case "Margin":
                            resourceValue = AutoScalePadding((Padding)resourceValue);
                            break;
                        case "ClientSize":
                            if (control is Form && PreserveFormSize) continue;
                            resourceValue = AutoScaleSize((Size)resourceValue);
                            break;
                    }
                }

                // use the property descriptor to set the resource value
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component).Find(propertyName, false);
                if (((propertyDescriptor != null) && !propertyDescriptor.IsReadOnly) &&
                    ((resourceValue == null) || propertyDescriptor.PropertyType.IsInstanceOfType(resourceValue)))
                {
                    try
                    {
                        propertyDescriptor.SetValue(component, resourceValue);
                    }
                    catch (Exception e)
                    {
                        string error = e.GetType().Name + " - " + e.Message;
                    }
                }
                else
                {
                    // there was no property corresponding to the given resource name.  If this is a control
                    // the property may be an extender property so try applying it as an extender resource
                    if (control != null)
                    {
                        ApplyExtenderResource(extenderProviders, control, propertyName, resourceValue);
                    }
                }
            }
        }

        #endregion
    }
}