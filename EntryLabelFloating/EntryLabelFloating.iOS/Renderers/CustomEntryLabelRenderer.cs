using UIKit;
using System;
using CoreGraphics;
using Xamarin.Forms;
using CoreAnimation;
using System.Drawing;
using Xamarin.Forms.Platform.iOS;
using EntryLabelFloating.Custom;
using EntryLabelFloating.iOS.Renderers;

[assembly: ExportRenderer(typeof(CustomEntryLabel), typeof(CustomEntryLabelRenderer))]
namespace EntryLabelFloating.iOS.Renderers
{
	public class CustomEntryLabelRenderer : EntryRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null || this.Element == null) return;

			var element = (CustomEntryLabel)this.Element;
			var textField = this.Control;

			if (e.PropertyName == CustomEntryLabel.IsEntrySelectedProperty.PropertyName)
			{
				if (((CustomEntryLabel)this.Element).IsEntrySelected)
				{
					this.Control.Layer.BorderColor = UIColor.Blue.CGColor;
					this.Control.Layer.BorderWidth = new nfloat(0.8);
					this.Control.Layer.CornerRadius = 5;
				}
				else
				{
					this.Control.Layer.BorderColor = UIColor.LightGray.CGColor;
					this.Control.Layer.CornerRadius = 5;
					this.Control.Layer.BorderWidth = new nfloat(0.8);
				}

			}
			if (!string.IsNullOrEmpty(element.EntryImage))
			{
				switch (element.EntryImageAlignment)
				{
					case ImageAlignment.Left:
						textField.LeftViewMode = UITextFieldViewMode.Always;
						textField.LeftView = GetImageView(element.EntryImage, element.EntryImageHeight, element.EntryImageWidth);
						break;
					case ImageAlignment.Right:
						textField.RightViewMode = UITextFieldViewMode.Always;
						textField.RightView = GetImageView(element.EntryImage, element.EntryImageHeight, element.EntryImageWidth);
						break;
				}
			}

			textField.BorderStyle = UITextBorderStyle.None;
			CALayer bottomBorder = new CALayer
			{
				Frame = new CGRect(0.0f, element.HeightRequest - 1, this.Frame.Width, 1.0f),
				BorderWidth = 2.0f,
				BorderColor = element.EntryLineColor.ToCGColor()
			};

			textField.Layer.AddSublayer(bottomBorder);
			textField.Layer.MasksToBounds = true;
		}

		private UIView GetImageView(string imagePath, int height, int width)
		{
			var uiImageView = new UIImageView(UIImage.FromBundle(imagePath))
			{
				Frame = new RectangleF(0, 0, width, height)
			};
			UIView objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, width + 10, height));
			objLeftView.AddSubview(uiImageView);

			return objLeftView;
		}
	}
}