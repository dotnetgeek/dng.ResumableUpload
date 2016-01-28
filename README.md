# Resumable Upload for large files

This project is a server side implementation for the javascript libary https://github.com/flowjs/flow.js.
Right now it is feature complete and it works but a lot of improvements will be done in the future.

## How it works

This component is implemented as an IHttpHandler so that it can easily added to the ASP.NET Pipeline.

Most of the magic happens in the user's browser and will be handled by flow.js. On the server the component has to check
if the current uploaded chunk is  available or complete. Otherwise the chunk will be saved to disk. Every chunk will be saved
as a new file and after all chunks are uploaded the component will merge all chunks into a file. 

If e.g. the client will lost the connection, the upload can continue at the position where the last complete chunks ends.

Because of uploading small chunks (mostly smaller then 100MB) the upload-limit of the ISS / ASP.NET Pipeline
will not take place.


## ToDo
* Code rework
* Customizing FxCop rules
* Creating and publish samples
* Support CORS