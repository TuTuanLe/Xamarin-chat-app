package crc64c6bd226a92a79f74;


public class AndroidWebViewRenderer
	extends crc643f46942d9dd1fff9.WebViewRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("FrontendApp.Droid.AndroidWebViewRenderer, FrontendApp.Android", AndroidWebViewRenderer.class, __md_methods);
	}


	public AndroidWebViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == AndroidWebViewRenderer.class)
			mono.android.TypeManager.Activate ("FrontendApp.Droid.AndroidWebViewRenderer, FrontendApp.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public AndroidWebViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == AndroidWebViewRenderer.class)
			mono.android.TypeManager.Activate ("FrontendApp.Droid.AndroidWebViewRenderer, FrontendApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public AndroidWebViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == AndroidWebViewRenderer.class)
			mono.android.TypeManager.Activate ("FrontendApp.Droid.AndroidWebViewRenderer, FrontendApp.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
