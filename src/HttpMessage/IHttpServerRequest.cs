using System;
using System.Collections.Generic;
using Restup.HttpMessage.Models.Contracts;
using Restup.HttpMessage.Models.Schemas;

namespace Restup.HttpMessage
{
    public interface IHttpServerRequest
    {
        IEnumerable<IHttpRequestHeader> Headers { get; }
        HttpMethod? Method { get;  }
        Uri Uri { get;  }
        string HttpVersion { get;  }
        string ContentTypeCharset { get;  }
        IEnumerable<string> AcceptCharsets { get;  }
        int ContentLength { get;  }
        string ContentType { get;  }
        IEnumerable<string> AcceptEncodings { get; }
        IEnumerable<string> AcceptMediaTypes { get;  }
        byte[] Content { get;  }
        bool IsComplete { get;  }
    }
}