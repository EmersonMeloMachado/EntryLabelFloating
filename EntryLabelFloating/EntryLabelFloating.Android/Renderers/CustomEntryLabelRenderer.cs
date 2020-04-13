using Xamarin.Forms;
using Android.Content;
using Android.Graphics;
using Android.Content.Res;
using Android.Support.V4.Content;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using EntryLabelFloating.Custom;
using EntryLabelFloating.Droid.Renderers;

[assembly: ExportRenderer(typeof(CustomEntryLabel), typeof(CustomEntryLabelRenderer))]
namespace EntryLabelFloating.Droid.Renderers
{
    public class CustomEntryLabelRenderer : EntryRenderer
    {
        CustomEntryLabel element;

        public CustomEntryLabelRenderer(Context context) : base(context)
        { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            if (e.NewElement != null)
            {
                element = (CustomEntryLabel)Element;

                UpdateBorders(element);

                if (!string.IsNullOrEmpty(element.EntryImage))
                {
                    switch (element.EntryImageAlignment)
                    {
                        case ImageAlignment.Left:
                            Control.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.EntryImage), null, null, null);
                            break;
                        case ImageAlignment.Right:
                            Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.EntryImage), null);
                            break;
                    }
                }

                Control.CompoundDrawablePadding = 25;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            if (e.PropertyName == CustomEntryLabel.IsEntrySelectedProperty.PropertyName)
                UpdateBorders(element);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.EntryImageWidth * 2, element.EntryImageHeight * 2, true));
        }

        void UpdateBorders(CustomEntryLabel element)
        {
            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            shape.SetCornerRadius(10);

            if (element.IsEntrySelected)
                shape.SetStroke(10, Android.Graphics.Color.Blue);
            else
                shape.SetStroke(2, Android.Graphics.Color.ParseColor("#808080"));//shape.SetStroke(2, element.EntryBorderColor.ToAndroid());

            this.Control.SetBackground(shape);
        }
    }

}