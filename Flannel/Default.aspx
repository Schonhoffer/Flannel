<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head><title>
	Home Page
</title><link href="Styles/Site.css" rel="stylesheet" type="text/css" />

	
	<style>
		body {
			background-image: url('Images/blackplaidflannel.jpg');
		}
	</style>
</head>
<body>
<div class="aspNetHidden">
<input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
<input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUKMTY1NDU2MTA1MmQYAgUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgIFKWN0bDAwJEhlYWRMb2dpblZpZXckSGVhZExvZ2luU3RhdHVzJGN0bDAxBSljdGwwMCRIZWFkTG9naW5WaWV3JEhlYWRMb2dpblN0YXR1cyRjdGwwMwUTY3RsMDAkSGVhZExvZ2luVmlldw8PZAIBZH2n0XLZgdoy8eBncGmYX/TRZ3EFnadeOpEow/9yOhmc" />
</div>

<script type="text/javascript">
//<![CDATA[
	var theForm = document.forms['ctl01'];
	if (!theForm) {
		theForm = document.ctl01;
	}
	function __doPostBack(eventTarget, eventArgument) {
		if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
			theForm.__EVENTTARGET.value = eventTarget;
			theForm.__EVENTARGUMENT.value = eventArgument;
			theForm.submit();
		}
	}
//]]>
</script>



<script src="/WebResource.axd?d=WVG_FgdTWjKHCe5ZKk2NBYxOg4n5JMdySZ9IAbj0CWBH5t93z-KFtHKJemM0iT-6SYy0XA8ndnYxEDgRBaxMJuLwCtfvdiCjTrvho-pIyuc1&amp;t=634617937253024435" type="text/javascript"></script>
<div class="aspNetHidden">

	<input type="hidden" name="__EVENTVALIDATION" id="__EVENTVALIDATION" value="/wEWAgKNleXACgLC0ZzyCtRptfkuAAQCJOt54/jU+2TTCKw2ulYEVgKSkUcUHKGE" />
</div>
	<div class="page">
		<div class="header">
			<div class="title">
				<h1>
					My ASP.NET Application
				</h1>
			</div>
			<div class="loginDisplay">
				
						Welcome <span class="bold"><span id="HeadLoginView_HeadLoginName">IQMETRIXHO\Andrew Schonhoffer</span></span>!
						[ <a id="HeadLoginView_HeadLoginStatus" href="javascript:__doPostBack(&#39;ctl00$HeadLoginView$HeadLoginStatus$ctl00&#39;,&#39;&#39;)">Log Out</a> ]
					
			</div>
			<div class="clear hideSkiplink">
				<a href="#NavigationMenu_SkipLink"><img alt="Skip Navigation Links" src="/WebResource.axd?d=yZAUWZ-5vS4mzBB8b4L7EKkzQNVQzGaQcWY_A6IZRiVRvm7Ptbpmz0fi2Co31QrqxJq5kjpklsmmWsHgPhlZlxI6ZL3aXULftO60lGlIuwI1&amp;t=634617937253024435" width="0" height="0" style="border-width:0px;" /></a><div class="menu" id="NavigationMenu">
	<ul class="level1">
		<li><a class="level1" href="Default.aspx">Home</a></li><li><a class="level1" href="About.aspx">About</a></li>
	</ul>
</div><a id="NavigationMenu_SkipLink"></a>
			</div>
		</div>
		<div class="main">
			
    <h2>
        Appropriate Temporary Flannel!!
    </h2>
    <form method="post" action="/api/submit" enctype="multipart/form-data">
		<input type="file" name="filename"/>
		<input type="submit"/>
    </form>
	
	<form method="post" action="/api/voteup" >
		<input type="hidden" name="SubmissionID" value="8f4e0f4b-12a1-4cfe-b95c-0dabaf263b0b"/>
		<input type="submit"/>
    </form>

		</div>
		<div class="clear">
		</div>
	</div>
	<div class="footer">
		
	</div>
	
<script type='text/javascript'>	new Sys.WebForms.Menu({ element: 'NavigationMenu', disappearAfter: 500, orientation: 'horizontal', tabIndex: 0, disabled: false });</script>
</body>
</html>
