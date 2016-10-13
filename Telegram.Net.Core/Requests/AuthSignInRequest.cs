﻿using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSignInRequest : MTProtoRequest
    {
        private readonly string _phoneNumber;
        private readonly string _phoneCodeHash;
        private readonly string _code;
        public User user;
        public int SessionExpires;

        public AuthSignInRequest(string phoneNumber, string phoneCodeHash, string code)
        {
            _phoneNumber = phoneNumber;
            _phoneCodeHash = phoneCodeHash;
            _code = code;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0xbcd51581);
            Serializers.String.Write(writer, _phoneNumber);
            Serializers.String.Write(writer, _phoneCodeHash);
            Serializers.String.Write(writer, _code);
        }

        public override void OnResponse(BinaryReader reader)
        {
            var dataCode = reader.ReadUInt32(); //0xf6b673a4
            var expires = reader.ReadInt32();
            user = TL.Parse<User>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}