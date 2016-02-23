using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.DemoControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public class FileSample
    {

        [UriFormat("/file/{filename}")]
        public SendFile GetSingletonSampleValue(string filename)
        {
            // if no path, then default directory is ApplicationData.Current.LocalFolder
            // so please place your files in this folder or specify a folder where you have the right
            // relative path is supported as well meanining you can do /file/folder1/file.css
            // it will look at the file folder1\file.css in the LocalFolder path or a path you have the right
            var filetoserve = Uri.UnescapeDataString(filename);
            var test = new SendFile(
                SendFile.ResponseStatus.OK,
                 filetoserve);
            return test;
        }

        [UriFormat("/path/{path}/file/{filename}")]
        public SendFile GetSingletonSampleValue(string path, string filename)
        {
            var filetoserve = Uri.UnescapeDataString(filename);
            var pathtoserve = Uri.UnescapeDataString(path);
            var test = new SendFile(
                SendFile.ResponseStatus.OK,
                 new File(filetoserve, pathtoserve));
            return test;
        }
    }
}
