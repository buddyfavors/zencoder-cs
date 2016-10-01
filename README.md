# Zencoder .NET
#### A .NET C# client library for the [zencoder.com](http://zencoder.com/) API

Visit <https://app.zencoder.com/docs> for full API documentation.

This is an attempt at a fully object-oriented implementation of the Zencoder API for .NET. 
It is compatible with .NET Core.

XML comments are used extensively throughout, so it should be pretty discoverable if you're
using an IDE that supports intellisense.

## Building

You'll need .NET Core SDK and VS 2015 Tooling (https://www.microsoft.com/net/core).

## Basic Usage

All of the requests can be accessed through a `Zencoder` object instance. Construct one with
your API key:

    var zen = new Zencoder(api_key);
    
You can then create a job:

	zen.CreateJobAsync("s3://bucket-name/file-name.avi", new Output[] {
		// Define your output(s) here.
	});

There is also full non-blocking (async) support, so the non-blocking version of the above call is:

	zen.CreateJob("s3://bucket-name/file-name.avi", new Output[] {
		// Define your output(s) here.
	}, response => {
		// Work with the response here.
	});

It's not much harder to work with each API action's requestion & response objects, so feel free to
rock it that way if you prefer.

In this fork I've updated most of the methods to be Async.

**Please note:** the above input file URLs (`s3://bucket-name/file-name.avi`) are only examples. Zencoder
supports a number of input and output location types (e.g., HTTP, S3, Cloud Files, FTP and SFTP). Please 
[see the Zencoder documentation](https://app.zencoder.com/docs/api/encoding/job/input) for more information.

## HTTP Notifications

TODO Example for ASP.NET Core Web Application

## License

Licensed under the [MIT](http://www.opensource.org/licenses/mit-license.html) license. See LICENSE.txt.

Copyright (c) 2010 Chad Burggraf.
Copyright (c) 2016 Buddy Favors Jr.
