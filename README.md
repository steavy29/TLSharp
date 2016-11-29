# Telegram.Net

[![Build status](https://ci.appveyor.com/api/projects/status/e2eo2ltjc5pygvtp?svg=true)](https://ci.appveyor.com/project/steavy29/telegram-net)

Unofficial Telegram (http://telegram.org) client library implemented in C#.

**Supported features:**
* Library supports API Layer 23 (latest officially documented Api. See https://core.telegram.org/schema)
* Automatic reconnection on network absence
* Main requests are implemented as methods in ```TelegramClient```. If your one isn't yet there, just use ```SendRpcRequest```
* Update notifications handling

**Upcoming features:**
* Session storing in custom format
* Convert library to PCL
* Handle bad system time settings

# Table of contents?

- [How do I add this to my project?](#how-do-i-add-this-to-my-project)
- [Starter Guide](#starter-guide)
- [FAQ](#faq)
- [License](#license)

# How do I add this to my project?

Library isn't ready for production usage, that's why no Nu-Get package available.

To use it follow next steps:

1. Clone Telegram.Net from GitHub
1. Compile source with VS2015
1. Add reference to ```Telegram.Net.Core.dll``` to your project.

# Starter Guide

### Quick Configuration
Telegram API isn't that easy to start. You need to do some configuration first.

1. Create a [developer account](https://my.telegram.org/) in Telegram. 
1. Goto [API development tools](https://my.telegram.org/apps) and copy **API_ID** and **API_HASH** from your account. You'll need it later.

### Initializing client

To initialize client you need to implement your own instance of ```ISessionStore``` which Telegram.Net will use to save Session info.

```
class MySesionStore: ISessionStore
{
	//...
}
```

Next, create client instance and pass all required parameters. You need your **API_ID** and **API_HASH** for this step.

```
var client = new TelegramClient(sessionStore, apiId, apiHash);
```

If you are going to handle Update notifications from Telegram server or network connection state, then you should subscribe to appropriate events.

```
client.UpdateMessage += UpdateHandler;
client.ConnectionStateChanged += ConnectionHandler;
```

And finally you need to call ```Start``` method. It returns ```bool``` indicating if client has successfully connected to the server. If not, library will automatically try to reconnect every 8 seconds.

Now you can call methods.

All methods except ```IsPhoneRegistered``` requires an authenticated user. Example usage of all methods you can find in [Tests].

### FAQ

#### I get an exception: "Channel was closed"

This can happen when sending your request was successfull but before receiving the response network connection was lost. This should be considered as missing internet connection situation.

#### Why I get FLOOD_WAIT error?
It's Telegram restrictions. See [this](https://core.telegram.org/api/errors#420-flood)

## License

The MIT License

Copyright (c) 2016 Stepan Tyzhai

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Copy of the parental license.
>The MIT License
>
>Copyright (c) 2015 Ilya Pirozhenko http://www.sochix.ru/
>
>Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
>
>The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
>
>THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.