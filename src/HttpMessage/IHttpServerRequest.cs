using System;
using System.Collections.Generic;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;

namespace Devkoes.HttpMessage
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
        MediaType? ContentType { get;  }
        IEnumerable<MediaType> AcceptMediaTypes { get;  }
        byte[] Content { get;  }
        bool IsComplete { get;  }
    }
}