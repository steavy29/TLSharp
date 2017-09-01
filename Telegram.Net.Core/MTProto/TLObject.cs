using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Telegram.Net.Core.MTProto
{
    public abstract class TLObject
    {
        public static readonly int apiLayer = 23;

        public static readonly uint boolFalse = 0xbc799737;
        public static readonly uint boolTrue = 0x997275b5;
        public static readonly uint vectorCode = 0x1cb5c415;
        
        public abstract Constructor constructor { get; }

        public abstract void Write(BinaryWriter writer);
        public abstract void Read(BinaryReader reader);

        #region Static Context

        protected static readonly Dictionary<uint, Type> constructors = new Dictionary<uint, Type>
        {
            {0xc4b9f9bb, typeof (ErrorConstructor)},
            {0x7f3b18ea, typeof (InputPeerEmptyConstructor)},
            {0x7da07ec9, typeof (InputPeerSelfConstructor)},
            {0x1023dbe8, typeof (InputPeerContactConstructor)},
            {0x9b447325, typeof (InputPeerForeignConstructor)},
            {0x179be863, typeof (InputPeerChatConstructor)},
            {0xb98886cf, typeof (InputUserEmptyConstructor)},
            {0xf7c1b13f, typeof (InputUserSelfConstructor)},
            {0x86e94f65, typeof (InputUserContactConstructor)},
            {0x655e74ff, typeof (InputUserForeignConstructor)},
            {0xf392b7f4, typeof (InputPhoneContactConstructor)},
            {0xf52ff27f, typeof (InputFileConstructor)},
            {0x9664f57f, typeof (InputMediaEmptyConstructor)},
            {0x2dc53a7d, typeof (InputMediaUploadedPhotoConstructor)},
            {0x8f2ab2ec, typeof (InputMediaPhotoConstructor)},
            {0xf9c44144, typeof (InputMediaGeoPointConstructor)},
            {0xa6e45987, typeof (InputMediaContactConstructor)},
            {0x4847d92a, typeof (InputMediaUploadedVideoConstructor)},
            {0xe628a145, typeof (InputMediaUploadedThumbVideoConstructor)},
            {0x7f023ae6, typeof (InputMediaVideoConstructor)},
            {0x1ca48f57, typeof (InputChatPhotoEmptyConstructor)},
            {0x94254732, typeof (InputChatUploadedPhotoConstructor)},
            {0xb2e1bf08, typeof (InputChatPhotoConstructor)},
            {0xe4c123d6, typeof (InputGeoPointEmptyConstructor)},
            {0xf3b7acc9, typeof (InputGeoPointConstructor)},
            {0x1cd7bf0d, typeof (InputPhotoEmptyConstructor)},
            {0xfb95c6c4, typeof (InputPhotoConstructor)},
            {0x5508ec75, typeof (InputVideoEmptyConstructor)},
            {0xee579652, typeof (InputVideoConstructor)},
            {0x14637196, typeof (InputFileLocationConstructor)},
            {0x3d0364ec, typeof (InputVideoFileLocationConstructor)},
            {0xade6b004, typeof (InputPhotoCropAutoConstructor)},
            {0xd9915325, typeof (InputPhotoCropConstructor)},
            {0x770656a8, typeof (InputAppEventConstructor)},
            {0x9db1bc6d, typeof (PeerUserConstructor)},
            {0xbad0e5bb, typeof (PeerChatConstructor)},
            {0xaa963b05, typeof (StorageFileUnknownConstructor)},
            {0x007efe0e, typeof (StorageFileJpegConstructor)},
            {0xcae1aadf, typeof (StorageFileGifConstructor)},
            {0x0a4f63c0, typeof (StorageFilePngConstructor)},
            {0x528a0677, typeof (StorageFileMp3Constructor)},
            {0x4b09ebbc, typeof (StorageFileMovConstructor)},
            {0x40bc6f52, typeof (StorageFilePartialConstructor)},
            {0xb3cea0e4, typeof (StorageFileMp4Constructor)},
            {0x1081464c, typeof (StorageFileWebpConstructor)},
            {0x7c596b46, typeof (FileLocationUnavailableConstructor)},
            {0x53d69076, typeof (FileLocationConstructor)},
            {0x200250ba, typeof (UserEmptyConstructor)},
            {0x7007b451, typeof (UserSelfConstructor)},
            {0xcab35e18, typeof (UserContactConstructor)},
            {0xd9ccc4ef, typeof (UserRequestConstructor)},
            {0x075cf7a8, typeof (UserForeignConstructor)},
            {0xd6016d7a, typeof (UserDeletedConstructor)},
            {0x4f11bae1, typeof (UserProfilePhotoEmptyConstructor)},
            {0xd559d8c8, typeof (UserProfilePhotoConstructor)},
            {0x09d05049, typeof (UserStatusEmptyConstructor)},
            {0xedb93949, typeof (UserStatusOnlineConstructor)},
            {0x008c703f, typeof (UserStatusOfflineConstructor)},
            {0xe26f42f1, typeof (UserStatusRecentlyConstructor)},
            {0x07bf09fc, typeof (UserStatusLastWeekConstructor)},
            {0x77ebc742, typeof (UserStatusLastMonthConstructor)},
            {0x9ba2d800, typeof (ChatEmptyConstructor)},
            {0x6e9c9bc7, typeof (ChatConstructor)},
            {0xfb0ccc41, typeof (ChatForbiddenConstructor)},
            {0x630e61be, typeof (ChatFullConstructor)},
            {0xc8d7493e, typeof (ChatParticipantConstructor)},
            {0x0fd2bb8a, typeof (ChatParticipantsForbiddenConstructor)},
            {0x7841b415, typeof (ChatParticipantsConstructor)},
            {0x37c1011c, typeof (ChatPhotoEmptyConstructor)},
            {0x6153276a, typeof (ChatPhotoConstructor)},
            {0x83e5de54, typeof (MessageEmptyConstructor)},
            {0x567699B3, typeof (MessageConstructor)},
            {0xa367e716, typeof (MessageForwardedConstructor)},
            {0x1d86f70e, typeof (MessageServiceConstructor)},
            {0x3ded6320, typeof (MessageMediaEmptyConstructor)},
            {0xc8c45a2a, typeof (MessageMediaPhotoConstructor)},
            {0xa2d24290, typeof (MessageMediaVideoConstructor)},
            {0x56e0d474, typeof (MessageMediaGeoConstructor)},
            {0x5e7d2f39, typeof (MessageMediaContactConstructor)},
            {0x29632a36, typeof (MessageMediaUnsupportedConstructor)},
            {0xb6aef7b0, typeof (MessageActionEmptyConstructor)},
            {0xa6638b9a, typeof (MessageActionChatCreateConstructor)},
            {0xb5a1ce5a, typeof (MessageActionChatEditTitleConstructor)},
            {0x7fcb13a8, typeof (MessageActionChatEditPhotoConstructor)},
            {0x95e3fbef, typeof (MessageActionChatDeletePhotoConstructor)},
            {0x5e3cfc4b, typeof (MessageActionChatAddUserConstructor)},
            {0xb2ae9b0c, typeof (MessageActionChatDeleteUserConstructor)},
            {0x214a8cdf, typeof (DialogConstructor)},
            {0x2331b22d, typeof (PhotoEmptyConstructor)},
            {0x22b56751, typeof (PhotoConstructor)},
            {0x0e17e23c, typeof (PhotoSizeEmptyConstructor)},
            {0x77bfb61b, typeof (PhotoSizeConstructor)},
            {0xe9a734fa, typeof (PhotoCachedSizeConstructor)},
            {0xc10658a8, typeof (VideoEmptyConstructor)},
            {0x388fa391, typeof (VideoConstructor)},
            {0x1117dd5f, typeof (GeoPointEmptyConstructor)},
            {0x2049d70c, typeof (GeoPointConstructor)},
            {0xe300cc3b, typeof (AuthCheckedPhoneConstructor)},
            {0xefed51d9, typeof (AuthSentCodeConstructor)},
            {0xe325edcf, typeof (AuthSentAppCodeConstructor)},
            {0xf6b673a4, typeof (AuthAuthorizationConstructor)},
            {0xdf969c2d, typeof (AuthExportedAuthorizationConstructor)},
            {0xb8bc5b0c, typeof (InputNotifyPeerConstructor)},
            {0x193b4417, typeof (InputNotifyUsersConstructor)},
            {0x4a95e84e, typeof (InputNotifyChatsConstructor)},
            {0xa429b886, typeof (InputNotifyAllConstructor)},
            {0xf03064d8, typeof (InputPeerNotifyEventsEmptyConstructor)},
            {0xe86a2c74, typeof (InputPeerNotifyEventsAllConstructor)},
            {0x46a2ce98, typeof (InputPeerNotifySettingsConstructor)},
            {0xadd53cb3, typeof (PeerNotifyEventsEmptyConstructor)},
            {0x6d1ded88, typeof (PeerNotifyEventsAllConstructor)},
            {0x70a68512, typeof (PeerNotifySettingsEmptyConstructor)},
            {0x8d5e11ee, typeof (PeerNotifySettingsConstructor)},
            {0xccb03657, typeof (WallPaperConstructor)},
            {0x771095da, typeof (UserFullConstructor)},
            {0xf911c994, typeof (ContactConstructor)},
            {0xd0028438, typeof (ImportedContactConstructor)},
            {0x561bc879, typeof (ContactBlockedConstructor)},
            {0xea879f95, typeof (ContactFoundConstructor)},
            {0x3de191a1, typeof (ContactSuggestedConstructor)},
            {0xaa77b873, typeof (ContactStatusConstructor)},
            {0x3631cf4c, typeof (ChatLocatedConstructor)},
            {0x133421f8, typeof (ContactsForeignLinkUnknownConstructor)},
            {0xa7801f47, typeof (ContactsForeignLinkRequestedConstructor)},
            {0x1bea8ce1, typeof (ContactsForeignLinkMutualConstructor)},
            {0xd22a1c60, typeof (ContactsMyLinkEmptyConstructor)},
            {0x6c69efee, typeof (ContactsMyLinkRequestedConstructor)},
            {0xc240ebd9, typeof (ContactsMyLinkContactConstructor)},
            {0xeccea3f5, typeof (ContactsLinkConstructor)},
            {0xb74ba9d2, typeof (ContactsContactsNotModifiedConstructor)},
            {0x6f8b8cb2, typeof (ContactsContactsConstructor)},
            {0xad524315, typeof (ContactsImportedContactsConstructor)},
            {0x1c138d15, typeof (ContactsBlockedConstructor)},
            {0x900802a1, typeof (ContactsBlockedSliceConstructor)},
            {0x0566000e, typeof (ContactsFoundConstructor)},
            {0x5649dcc5, typeof (ContactsSuggestedConstructor)},
            {0x15ba6c40, typeof (MessagesDialogsConstructor)},
            {0x71e094f3, typeof (MessagesDialogsSliceConstructor)},
            {0x8c718e87, typeof (MessagesMessagesConstructor)},
            {0x0b446ae3, typeof (MessagesMessagesSliceConstructor)},
            {0x3f4e0648, typeof (MessagesMessageEmptyConstructor)},
            {0xff90c417, typeof (MessagesMessageConstructor)},
            {0x969478bb, typeof (MessagesStatedMessagesConstructor)},
            {0xd07ae726, typeof (MessagesStatedMessageConstructor)},
            {0xd1f4d35c, typeof (SentMessageConstructor)},
            {0x40e9002a, typeof (MessagesChatConstructor)},
            {0x8150cbd8, typeof (MessagesChatsConstructor)},
            {0xe5d7d19c, typeof (MessagesChatFullConstructor)},
            {0xb7de36f2, typeof (MessagesAffectedHistoryConstructor)},
            {0x57e2f66c, typeof (InputMessagesFilterEmptyConstructor)},
            {0x9609a51c, typeof (InputMessagesFilterPhotosConstructor)},
            {0x9fc00e65, typeof (InputMessagesFilterVideoConstructor)},
            {0x56e9f0e4, typeof (InputMessagesFilterPhotoVideoConstructor)},
            {0x013abdb3, typeof (UpdateNewMessageConstructor)},
            {0x4e90bfd6, typeof (UpdateMessageIdConstructor)},
            {0xc6649e31, typeof (UpdateReadMessagesConstructor)},
            {0xa92bfe26, typeof (UpdateDeleteMessagesConstructor)},
            {0xd15de04d, typeof (UpdateRestoreMessagesConstructor)},
            {0x5c486927, typeof (UpdateUserTypingConstructor)},
            {0x9a65ea1f, typeof (UpdateChatUserTypingConstructor)},
            {0x07761198, typeof (UpdateChatParticipantsConstructor)},
            {0x1bfbd823, typeof (UpdateUserStatusConstructor)},
            {0xa7332b73, typeof (UpdateUserNameConstructor)},
            {0x95313b0c, typeof (UpdateUserPhotoConstructor)},
            {0x2575bbb9, typeof (UpdateContactRegisteredConstructor)},
            {0x51a48a9a, typeof (UpdateContactLinkConstructor)},
            {0x6f690963, typeof (UpdateActivationConstructor)},
            {0x8f06529a, typeof (UpdateNewAuthorizationConstructor)},
            {0xa56c2a3e, typeof (UpdatesStateConstructor)},
            {0x5d75a138, typeof (UpdatesDifferenceEmptyConstructor)},
            {0x00f49ca0, typeof (UpdatesDifferenceConstructor)},
            {0xa8fb1981, typeof (UpdatesDifferenceSliceConstructor)},
            {0xe317af7e, typeof (UpdatesTooLongConstructor)},
            {0xd3f45784, typeof (UpdateShortMessageConstructor)},
            {0x2b2fbd4e, typeof (UpdateShortChatMessageConstructor)},
            {0x78d4dec1, typeof (UpdateShortConstructor)},
            {0x725b04c3, typeof (UpdatesCombinedConstructor)},
            {0x74ae4240, typeof (UpdatesConstructor)},
            {0x8dca6aa5, typeof (PhotosPhotosConstructor)},
            {0x15051f54, typeof (PhotosPhotosSliceConstructor)},
            {0x20212ca8, typeof (PhotosPhotoConstructor)},
            {0x096a18d5, typeof (UploadFileConstructor)},
            {0x2ec2a43c, typeof (DcOptionConstructor)},
            {0x7dae33e0, typeof (ConfigConstructor)},
            {0x8e1a1775, typeof (NearestDcConstructor)},
            {0x8987f311, typeof (HelpAppUpdateConstructor)},
            {0xc45a6536, typeof (HelpNoAppUpdateConstructor)},
            {0x18cb9f78, typeof (HelpInviteTextConstructor)},
            {0x3e74f5c6, typeof (MessagesStatedMessagesLinksConstructor)},
            {0xa9af2881, typeof (MessagesStatedMessageLinkConstructor)},
            {0xe9db4a3f, typeof (SentMessageLinkConstructor)},
            {0x74d456fa, typeof (InputGeoChatConstructor)},
            {0x4d8ddec8, typeof (InputNotifyGeoChatPeerConstructor)},
            {0x75eaea5a, typeof (GeoChatConstructor)},
            {0x60311a9b, typeof (GeoChatMessageEmptyConstructor)},
            {0x4505f8e1, typeof (GeoChatMessageConstructor)},
            {0xd34fa24e, typeof (GeoChatMessageServiceConstructor)},
            {0x17b1578b, typeof (GeochatsStatedMessageConstructor)},
            {0x48feb267, typeof (GeochatsLocatedConstructor)},
            {0xd1526db1, typeof (GeochatsMessagesConstructor)},
            {0xbc5863e8, typeof (GeochatsMessagesSliceConstructor)},
            {0x6f038ebc, typeof (MessageActionGeoChatCreateConstructor)},
            {0x0c7d53de, typeof (MessageActionGeoChatCheckinConstructor)},
            {0x5a68e3f7, typeof (UpdateNewGeoChatMessageConstructor)},
            {0x63117f24, typeof (WallPaperSolidConstructor)},
            {0x12bcbd9a, typeof (UpdateNewEncryptedMessageConstructor)},
            {0x1710f156, typeof (UpdateEncryptedChatTypingConstructor)},
            {0xb4a2e88d, typeof (UpdateEncryptionConstructor)},
            {0x38fe25b7, typeof (UpdateEncryptedMessagesReadConstructor)},
            {0xab7ec0a0, typeof (EncryptedChatEmptyConstructor)},
            {0x3bf703dc, typeof (EncryptedChatWaitingConstructor)},
            {0xfda9a7b7, typeof (EncryptedChatRequestedConstructor)},
            {0x6601d14f, typeof (EncryptedChatConstructor)},
            {0x13d6dd27, typeof (EncryptedChatDiscardedConstructor)},
            {0xf141b5e1, typeof (InputEncryptedChatConstructor)},
            {0xc21f497e, typeof (EncryptedFileEmptyConstructor)},
            {0x4a70994c, typeof (EncryptedFileConstructor)},
            {0x1837c364, typeof (InputEncryptedFileEmptyConstructor)},
            {0x64bd0306, typeof (InputEncryptedFileUploadedConstructor)},
            {0x5a17b5e5, typeof (InputEncryptedFileConstructor)},
            {0xf5235d55, typeof (InputEncryptedFileLocationConstructor)},
            {0xed18c118, typeof (EncryptedMessageConstructor)},
            {0x23734b06, typeof (EncryptedMessageServiceConstructor)},
            {0x99a438cf, typeof (DecryptedMessageLayerConstructor)},
            {0x1f814f1f, typeof (DecryptedMessageConstructor)},
            {0xaa48327d, typeof (DecryptedMessageServiceConstructor)},
            {0x089f5c4a, typeof (DecryptedMessageMediaEmptyConstructor)},
            {0x32798a8c, typeof (DecryptedMessageMediaPhotoConstructor)},
            {0x4cee6ef3, typeof (DecryptedMessageMediaVideoConstructor)},
            {0x35480a59, typeof (DecryptedMessageMediaGeoPointConstructor)},
            {0x588a0a97, typeof (DecryptedMessageMediaContactConstructor)},
            {0xa1733aec, typeof (DecryptedMessageActionSetMessageTtlConstructor)},
            {0xc0e24635, typeof (MessagesDhConfigNotModifiedConstructor)},
            {0x2c221edd, typeof (MessagesDhConfigConstructor)},
            {0x560f8935, typeof (MessagesSentEncryptedMessageConstructor)},
            {0x9493ff32, typeof (MessagesSentEncryptedFileConstructor)},
            {0xfa4f0bb5, typeof (InputFileBigConstructor)},
            {0x2dc173c8, typeof (InputEncryptedFileBigUploadedConstructor)},
            {0x3a0eeb22, typeof (UpdateChatParticipantAddConstructor)},
            {0x6e5f8c22, typeof (UpdateChatParticipantDeleteConstructor)},
            {0x8e5e9873, typeof (UpdateDcOptionsConstructor)},
            {0x80ece81a, typeof (UpdateUserBlockedConstructor)},
            {0xbec268ef, typeof (UpdateNotifySettingsConstructor)},
            {0x382dd3e4, typeof (UpdateServiceNotificationConstructor)},
            {0xee3b272a, typeof (UpdatePrivacyConstructor)},
            {0x12b9417b, typeof (UpdateUserPhoneConstructor)},
            {0x61a6d436, typeof (InputMediaUploadedAudioConstructor)},
            {0x89938781, typeof (InputMediaAudioConstructor)},
            {0x34e794bd, typeof (InputMediaUploadedDocumentConstructor)},
            {0x3e46de5d, typeof (InputMediaUploadedThumbDocumentConstructor)},
            {0xd184e841, typeof (InputMediaDocumentConstructor)},
            {0x2fda2204, typeof (MessageMediaDocumentConstructor)},
            {0xc6b68300, typeof (MessageMediaAudioConstructor)},
            {0xd95adc84, typeof (InputAudioEmptyConstructor)},
            {0x77d440ff, typeof (InputAudioConstructor)},
            {0x72f0eaae, typeof (InputDocumentEmptyConstructor)},
            {0x18798952, typeof (InputDocumentConstructor)},
            {0x74dc404d, typeof (InputAudioFileLocationConstructor)},
            {0x4e45abe9, typeof (InputDocumentFileLocationConstructor)},
            {0xb095434b, typeof (DecryptedMessageMediaDocumentConstructor)},
            {0x6080758f, typeof (DecryptedMessageMediaAudioConstructor)},
            {0x586988d8, typeof (AudioEmptyConstructor)},
            {0xc7ac6496, typeof (AudioConstructor)},
            {0x36f8c871, typeof (DocumentEmptyConstructor)},
            {0xf9a39f4f, typeof (DocumentConstructor)},
            {0xab3a99ac, typeof (DialogConstructor)},
            {0x6c37c15c, typeof (DocumentAttributeImageSizeConstructor)},
            {0x11b58939, typeof (DocumentAttributeAnimatedConstructor)},
            {0xfb0a5727, typeof (DocumentAttributeStickerConstructor)},
            {0x5910cccb, typeof (DocumentAttributeVideoConstructor)},
            {0x051448e5, typeof (DocumentAttributeAudioConstructor)},
            {0x15590068, typeof (DocumentAttributeFilenameConstructor)},
            {0xae636f24, typeof(DisabledFeature)},
            {0x347773c5, typeof(Pong) }
        };

        public static bool ReadBool(BinaryReader reader)
        {
            uint dataCode = reader.ReadUInt32();
            if (dataCode != boolTrue && dataCode != boolFalse)
                throw new Exception("Invalid bool dataCode");

            return dataCode == boolTrue;
        }
        public static void WriteBool(BinaryWriter writer, bool value)
        {
            writer.Write(value ? boolTrue : boolFalse);
        }

        public static T Read<T>(BinaryReader reader)
        {
            uint dataCode = reader.ReadUInt32();
            return Read<T>(reader, dataCode);
        }

        public static T Read<T>(BinaryReader reader, uint dataCode)
        {
            if (dataCode == 0x997275b5)
            {
                return (T)(object)true;
            }
            if (dataCode == 0xbc799737)
            {
                return (T)(object)false;
            }

            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            if (typeof(TLObject).GetTypeInfo().IsAssignableFrom(typeInfo))
            {
                if (!constructors.ContainsKey(dataCode))
                {
                    throw new Exception($"Invalid constructor code {dataCode.ToString("X")}");
                }

                Type constructorType = constructors[dataCode];
                if (!typeInfo.IsAssignableFrom(constructorType.GetTypeInfo()))
                {
                    throw new Exception($"Try to parse {typeInfo.FullName}, but incompatible type {constructorType.FullName}");
                }

                T obj = (T)Activator.CreateInstance(constructorType);
                ((TLObject)(object)obj).Read(reader);
                return obj;
            }

            throw new Exception("unknown return type");
        }

        public static List<T> ReadVector<T>(BinaryReader reader, bool readVectorDataCode = true) where T : TLObject
        {
            return ReadVector(reader, () => Read<T>(reader), readVectorDataCode);
        }
        public static void WriteVector<T>(BinaryWriter writer, List<T> vector) where T : TLObject
        {
            WriteVector(writer, vector, i => i.Write(writer));
        }

        public static List<T> ReadVector<T>(BinaryReader reader, Func<T> readNextFunc, bool readVectorDataCode = true)
        {
            if (readVectorDataCode)
            {
                var readVectorCode = reader.ReadInt32(); // vector code
                if (readVectorCode != vectorCode)
                    throw new Exception("Invalid vector code");
            }

            int count = reader.ReadInt32();
            var result = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(readNextFunc());
            }

            return result;
        }
        public static void WriteVector<T>(BinaryWriter writer, List<T> vector, Action<T> writeNextFunc)
        {
            writer.Write(vectorCode);

            writer.Write(vector.Count);
            foreach (var item in vector)
            {
                writeNextFunc(item);
            }
        }

        #endregion

        public T Cast<T>() where T : TLObject
        {
            return (T)this;
        }
        public T As<T>() where T : TLObject
        {
            return this as T;
        }
    }

    // all constructor types

    public enum Constructor
    {
        MessageUndelivered,
        Error,
        InputPeerEmpty,
        InputPeerSelf,
        InputPeerContact,
        InputPeerForeign,
        InputPeerChat,
        InputUserEmpty,
        InputUserSelf,
        InputUserContact,
        InputUserForeign,
        InputPhoneContact,
        InputFile,
        InputMediaEmpty,
        InputMediaUploadedPhoto,
        InputMediaPhoto,
        InputMediaGeoPoint,
        InputMediaContact,
        InputMediaUploadedVideo,
        InputMediaUploadedThumbVideo,
        InputMediaVideo,
        InputChatPhotoEmpty,
        InputChatUploadedPhoto,
        InputChatPhoto,
        InputGeoPointEmpty,
        InputGeoPoint,
        InputPhotoEmpty,
        InputPhoto,
        InputVideoEmpty,
        InputVideo,
        InputFileLocation,
        InputVideoFileLocation,
        InputPhotoCropAuto,
        InputPhotoCrop,
        InputAppEvent,
        PeerUser,
        PeerChat,
        StorageFileUnknown,
        StorageFileJpeg,
        StorageFileGif,
        StorageFilePng,
        StorageFileMp3,
        StorageFileMov,
        StorageFilePartial,
        StorageFileMp4,
        StorageFileWebp,
        FileLocationUnavailable,
        FileLocation,
        UserEmpty,
        UserSelf,
        UserContact,
        UserRequest,
        UserForeign,
        UserDeleted,
        UserProfilePhotoEmpty,
        UserProfilePhoto,
        UserStatusEmpty,
        UserStatusOnline,
        UserStatusOffline,
        UserStatusRecently,
        UserStatusLastWeek,
        UserStatusLastMonth,
        ChatEmpty,
        Chat,
        ChatForbidden,
        ChatFull,
        ChatParticipant,
        ChatParticipantsForbidden,
        ChatParticipants,
        ChatPhotoEmpty,
        ChatPhoto,
        MessageEmpty,
        Message,
        MessageForwarded,
        MessageService,
        MessageMediaEmpty,
        MessageMediaPhoto,
        MessageMediaVideo,
        MessageMediaGeo,
        MessageMediaContact,
        MessageMediaUnsupported,
        MessageActionEmpty,
        MessageActionChatCreate,
        MessageActionChatEditTitle,
        MessageActionChatEditPhoto,
        MessageActionChatDeletePhoto,
        MessageActionChatAddUser,
        MessageActionChatDeleteUser,
        Dialog,
        PhotoEmpty,
        Photo,
        PhotoSizeEmpty,
        PhotoSize,
        PhotoCachedSize,
        VideoEmpty,
        Video,
        GeoPointEmpty,
        GeoPoint,
        AuthCheckedPhone,
        AuthSentCode,
        AuthSentAppCode,
        AuthAuthorization,
        AuthExportedAuthorization,
        InputNotifyPeer,
        InputNotifyUsers,
        InputNotifyChats,
        InputNotifyAll,
        InputPeerNotifyEventsEmpty,
        InputPeerNotifyEventsAll,
        InputPeerNotifySettings,
        PeerNotifyEventsEmpty,
        PeerNotifyEventsAll,
        PeerNotifySettingsEmpty,
        PeerNotifySettings,
        WallPaper,
        UserFull,
        Contact,
        ImportedContact,
        ContactBlocked,
        ContactFound,
        ContactSuggested,
        ContactStatus,
        ChatLocated,
        ContactsForeignLinkUnknown,
        ContactsForeignLinkRequested,
        ContactsForeignLinkMutual,
        ContactsMyLinkEmpty,
        ContactsMyLinkRequested,
        ContactsMyLinkContact,
        ContactsLink,
        ContactsContacts,
        ContactsContactsNotModified,
        ContactsImportedContacts,
        ContactsBlocked,
        ContactsBlockedSlice,
        ContactsFound,
        ContactsSuggested,
        MessagesDialogs,
        MessagesDialogsSlice,
        MessagesMessages,
        MessagesMessagesSlice,
        MessagesMessageEmpty,
        MessagesMessage,
        MessagesStatedMessages,
        MessagesStatedMessage,
        MessagesSentMessage,
        MessagesChat,
        MessagesChats,
        MessagesChatFull,
        MessagesAffectedHistory,
        InputMessagesFilterEmpty,
        InputMessagesFilterPhotos,
        InputMessagesFilterVideo,
        InputMessagesFilterPhotoVideo,
        UpdateNewMessage,
        UpdateMessageId,
        UpdateReadMessages,
        UpdateDeleteMessages,
        UpdateRestoreMessages,
        UpdateUserTyping,
        UpdateChatUserTyping,
        UpdateChatParticipants,
        UpdateUserStatus,
        UpdateUserName,
        UpdateUserPhoto,
        UpdateContactRegistered,
        UpdateContactLink,
        UpdateActivation,
        UpdateNewAuthorization,
        UpdatesState,
        UpdatesDifferenceEmpty,
        UpdatesDifference,
        UpdatesDifferenceSlice,
        UpdatesTooLong,
        UpdateShortMessage,
        UpdateShortChatMessage,
        UpdateShort,
        UpdatesCombined,
        Updates,
        PhotosPhotos,
        PhotosPhotosSlice,
        PhotosPhoto,
        UploadFile,
        DcOption,
        Config,
        NearestDc,
        HelpAppUpdate,
        HelpNoAppUpdate,
        HelpInviteText,
        MessagesStatedMessagesLinks,
        MessagesStatedMessageLink,
        MessagesSentMessageLink,
        InputGeoChat,
        InputNotifyGeoChatPeer,
        GeoChat,
        GeoChatMessageEmpty,
        GeoChatMessage,
        GeoChatMessageService,
        GeochatsStatedMessage,
        GeochatsLocated,
        GeochatsMessages,
        GeochatsMessagesSlice,
        MessageActionGeoChatCreate,
        MessageActionGeoChatCheckin,
        UpdateNewGeoChatMessage,
        WallPaperSolid,
        UpdateNewEncryptedMessage,
        UpdateEncryptedChatTyping,
        UpdateEncryption,
        UpdateEncryptedMessagesRead,
        EncryptedChatEmpty,
        EncryptedChatWaiting,
        EncryptedChatRequested,
        EncryptedChat,
        EncryptedChatDiscarded,
        InputEncryptedChat,
        EncryptedFileEmpty,
        EncryptedFile,
        InputEncryptedFileEmpty,
        InputEncryptedFileUploaded,
        InputEncryptedFile,
        InputEncryptedFileLocation,
        EncryptedMessage,
        EncryptedMessageService,
        DecryptedMessageLayer,
        DecryptedMessage,
        DecryptedMessageService,
        DecryptedMessageMediaEmpty,
        DecryptedMessageMediaPhoto,
        DecryptedMessageMediaVideo,
        DecryptedMessageMediaGeoPoint,
        DecryptedMessageMediaContact,
        DecryptedMessageActionSetMessageTtl,
        MessagesDhConfigNotModified,
        MessagesDhConfig,
        MessagesSentEncryptedMessage,
        MessagesSentEncryptedFile,
        InputFileBig,
        InputEncryptedFileBigUploaded,
        UpdateChatParticipantAdd,
        UpdateChatParticipantDelete,
        UpdateDcOptions,
        UpdateUserBlocked,
        UpdateNotifySettings,
        UpdateServiceNotification,
        UpdatePrivacy,
        UpdateUserPhone,
        InputMediaUploadedAudio,
        InputMediaAudio,
        InputMediaUploadedDocument,
        InputMediaUploadedThumbDocument,
        InputMediaDocument,
        MessageMediaDocument,
        MessageMediaAudio,
        InputAudioEmpty,
        InputAudio,
        InputDocumentEmpty,
        InputDocument,
        InputAudioFileLocation,
        InputDocumentFileLocation,
        DecryptedMessageMediaDocument,
        DecryptedMessageMediaAudio,
        AudioEmpty,
        Audio,
        DocumentEmpty,
        Document,
        DocumentAttributeImageSize,
        DocumentAttributeAnimated,
        DocumentAttributeSticker,
        DocumentAttributeVideo,
        DocumentAttributeAudio,
        DocumentAttributeFilename,
        DisabledFeature,
        Pong
    }

    // abstract types
    public abstract class ContactsImportedContacts : TLObject
    {

    }

    public abstract class Peer : TLObject
    {

    }

    public abstract class InputVideo : TLObject
    {

    }

    public abstract class HelpInviteText : TLObject
    {

    }

    public abstract class UserStatus : TLObject
    {

    }

    public abstract class MessagesFilter : TLObject
    {

    }

    public abstract class Error : TLObject
    {

    }

    public abstract class Updates : TLObject
    {

    }

    public abstract class HelpAppUpdate : TLObject
    {

    }

    public abstract class InputEncryptedChat : TLObject
    {

    }

    public abstract class DecryptedMessage : TLObject
    {

    }

    public abstract class InputAudio : TLObject
    {

    }

    public abstract class ChatLocated : TLObject
    {

    }

    public abstract class PhotoSize : TLObject
    {

    }

    public abstract class MessagesSentEncryptedMessage : TLObject
    {

    }

    public abstract class MessageMedia : TLObject
    {

    }

    public abstract class InputDocument : TLObject
    {

    }

    public abstract class ImportedContact : TLObject
    {

    }

    public abstract class ContactBlocked : TLObject
    {

    }

    public abstract class Message : TLObject
    {

    }

    public abstract class InputNotifyPeer : TLObject
    {

    }

    public abstract class MessagesChatFull : TLObject
    {

    }

    public abstract class ChatParticipant : TLObject
    {

    }

    public abstract class InputPhoto : TLObject
    {

    }

    public abstract class DecryptedMessageMedia : TLObject
    {

    }

    public abstract class InputFileLocation : TLObject
    {

    }

    public abstract class InputEncryptedFile : TLObject
    {

    }

    public abstract class ContactsForeignLink : TLObject
    {

    }

    public abstract class Document : TLObject
    {

    }

    public abstract class UserFull : TLObject
    {

    }

    public abstract class MessagesMessage : TLObject
    {

    }

    public abstract class DcOption : TLObject
    {

    }

    public abstract class PhotosPhotos : TLObject
    {

    }

    public abstract class InputPeerNotifySettings : TLObject
    {

    }

    public abstract class ContactsSuggested : TLObject
    {

    }

    public abstract class InputGeoPoint : TLObject
    {

    }

    public abstract class InputGeoChat : TLObject
    {

    }

    public abstract class InputContact : TLObject
    {

    }

    public abstract class EncryptedFile : TLObject
    {

    }

    public abstract class PeerNotifySettings : TLObject
    {

    }

    public abstract class AuthAuthorization : TLObject
    {

    }

    public abstract class AuthCheckedPhone : TLObject
    {

    }

    public abstract class FileLocation : TLObject
    {

    }

    public abstract class MessagesChats : TLObject
    {

    }

    public abstract class ContactsLink : TLObject
    {

    }

    public abstract class MessagesStatedMessage : TLObject
    {

    }

    public abstract class GeochatsLocated : TLObject
    {

    }

    public abstract class UpdatesState : TLObject
    {

    }

    public abstract class StorageFileType : TLObject
    {

    }

    public abstract class GeochatsStatedMessage : TLObject
    {

    }

    public abstract class ContactFound : TLObject
    {

    }

    public abstract class Photo : TLObject
    {

    }

    public abstract class InputMedia : TLObject
    {

    }

    public abstract class PhotosPhoto : TLObject
    {

    }

    public abstract class InputFile : TLObject
    {

    }

    public abstract class AuthExportedAuthorization : TLObject
    {

    }

    public abstract class User : TLObject
    {

    }

    public abstract class NearestDc : TLObject
    {

    }

    public abstract class Video : TLObject
    {

    }

    public abstract class ContactsBlocked : TLObject
    {

    }

    public abstract class MessagesAffectedHistory : TLObject
    {

    }

    public abstract class MessagesChat : TLObject
    {

    }

    public abstract class Chat : TLObject
    {

    }

    public abstract class ChatParticipants : TLObject
    {

    }

    public abstract class InputAppEvent : TLObject
    {

    }

    public abstract class MessagesMessages : TLObject
    {

    }

    public abstract class MessagesDialogs : TLObject
    {

    }

    public abstract class InputPeer : TLObject
    {

    }

    public abstract class ChatPhoto : TLObject
    {

    }

    public abstract class ContactsMyLink : TLObject
    {

    }

    public abstract class InputChatPhoto : TLObject
    {

    }

    public abstract class SentMessage : TLObject
    {

    }

    public abstract class MessagesStatedMessages : TLObject
    {

    }

    public abstract class UserProfilePhoto : TLObject
    {

    }

    public abstract class UpdatesDifference : TLObject
    {

    }

    public abstract class Update : TLObject
    {

    }

    public abstract class GeoPoint : TLObject
    {

    }

    public abstract class WallPaper : TLObject
    {

    }

    public abstract class DecryptedMessageLayer : TLObject
    {

    }

    public abstract class Config : TLObject
    {

    }

    public abstract class EncryptedMessage : TLObject
    {

    }

    public abstract class Dialog : TLObject
    {

    }

    public abstract class ContactStatus : TLObject
    {

    }

    public abstract class InputPeerNotifyEvents : TLObject
    {

    }

    public abstract class MessageAction : TLObject
    {

    }

    public abstract class DecryptedMessageAction : TLObject
    {

    }

    public abstract class AuthSentCode : TLObject
    {
        public bool phoneRegistered;
        public string phoneCodeHash;
        public int sendCallTimeout;
        public bool isPassword;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2215bcbd);
            writer.Write(0x2215bcbd);

            writer.Write(phoneRegistered ? 0x997275b5 : 0xbc799737);
            Serializers.String.Write(writer, phoneCodeHash);
            writer.Write(sendCallTimeout);
            writer.Write(isPassword);
        }

        public override void Read(BinaryReader reader)
        {
            phoneRegistered = reader.ReadUInt32() == 0x997275b5;
            phoneCodeHash = Serializers.String.Read(reader);
            sendCallTimeout = reader.ReadInt32();
            isPassword = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(auth_sentCode phone_registered:{phoneRegistered} phone_code_hash:'{phoneCodeHash}')";
        }
    }

    public abstract class GeochatsMessages : TLObject
    {

    }

    public abstract class InputUser : TLObject
    {

    }

    public abstract class EncryptedChat : TLObject
    {

    }

    public abstract class ContactsContacts : TLObject
    {

    }

    public abstract class GeoChatMessage : TLObject
    {

    }

    public abstract class PeerNotifyEvents : TLObject
    {

    }

    public abstract class ContactsFound : TLObject
    {

    }

    public abstract class Audio : TLObject
    {

    }

    public abstract class ChatFull : TLObject
    {

    }

    public abstract class MessagesDhConfig : TLObject
    {

    }

    public abstract class Contact : TLObject
    {

    }

    public abstract class UploadFile : TLObject
    {

    }

    public abstract class InputPhotoCrop : TLObject
    {

    }

    public abstract class ContactSuggested : TLObject
    {

    }

    public abstract class DisabledFeature : TLObject
    {
        
    }

    // types implementations


    public class ErrorConstructor : Error
    {
        public int code;
        public string text;

        public ErrorConstructor()
        {

        }

        public ErrorConstructor(int code, string text)
        {
            this.code = code;
            this.text = text;
        }

        public override Constructor constructor => Constructor.Error;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc4b9f9bb);
            writer.Write(code);
            Serializers.String.Write(writer, text);
        }

        public override void Read(BinaryReader reader)
        {
            code = reader.ReadInt32();
            text = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(error code:{code} text:'{text}')";
        }
    }

    public class InputPeerEmptyConstructor : InputPeer
    {
        public override Constructor constructor => Constructor.InputPeerEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7f3b18ea);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputPeerEmpty)";
        }
    }

    public class InputPeerSelfConstructor : InputPeer
    {
        public override Constructor constructor => Constructor.InputPeerSelf;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7da07ec9);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputPeerSelf)";
        }
    }

    public class InputPeerContactConstructor : InputPeer
    {
        public int userId;

        public InputPeerContactConstructor()
        {

        }

        public InputPeerContactConstructor(int userId)
        {
            this.userId = userId;
        }

        public override Constructor constructor => Constructor.InputPeerContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1023dbe8);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputPeerContact userId:{userId})";
        }
    }

    public class InputPeerForeignConstructor : InputPeer
    {
        public int userId;
        public long accessHash;

        public InputPeerForeignConstructor()
        {

        }

        public InputPeerForeignConstructor(int userId, long accessHash)
        {
            this.userId = userId;
            this.accessHash = accessHash;
        }

        public override Constructor constructor => Constructor.InputPeerForeign;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9b447325);
            writer.Write(userId);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputPeerForeign userId:{userId} accessHash:{accessHash})";
        }
    }

    public class InputPeerChatConstructor : InputPeer
    {
        public int chatId;

        public InputPeerChatConstructor()
        {

        }

        public InputPeerChatConstructor(int chatId)
        {
            this.chatId = chatId;
        }

        public override Constructor constructor => Constructor.InputPeerChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x179be863);
            writer.Write(chatId);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputPeerChat chatId:{chatId})";
        }
    }

    public class InputUserEmptyConstructor : InputUser
    {
        public override Constructor constructor => Constructor.InputUserEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb98886cf);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputUserEmpty)";
        }
    }

    public class InputUserSelfConstructor : InputUser
    {
        public override Constructor constructor => Constructor.InputUserSelf;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf7c1b13f);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputUserSelf)";
        }
    }

    public class InputUserContactConstructor : InputUser
    {
        public int userId;

        public InputUserContactConstructor()
        {

        }

        public InputUserContactConstructor(int userId)
        {
            this.userId = userId;
        }

        public override Constructor constructor => Constructor.InputUserContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x86e94f65);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputUserContact userId:{userId})";
        }
    }

    public class InputUserForeignConstructor : InputUser
    {
        public int userId;
        public long accessHash;

        public InputUserForeignConstructor()
        {

        }

        public InputUserForeignConstructor(int userId, long accessHash)
        {
            this.userId = userId;
            this.accessHash = accessHash;
        }

        public override Constructor constructor => Constructor.InputUserForeign;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x655e74ff);
            writer.Write(userId);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputUserForeign userId:{userId} accessHash:{accessHash})";
        }
    }

    public class InputPhoneContactConstructor : InputContact
    {
        public long clientId;
        public string phone;
        public string firstName;
        public string lastName;

        public InputPhoneContactConstructor()
        {

        }

        public InputPhoneContactConstructor(long clientId, string phone, string firstName, string lastName)
        {
            this.clientId = clientId;
            this.phone = phone;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public override Constructor constructor => Constructor.InputPhoneContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf392b7f4);
            writer.Write(clientId);
            Serializers.String.Write(writer, phone);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
        }

        public override void Read(BinaryReader reader)
        {
            clientId = reader.ReadInt64();
            phone = Serializers.String.Read(reader);
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(inputPhoneContact clientId:{clientId} phone:'{phone}' firstName:'{firstName}' lastName:'{lastName}')";
        }
    }

    public class InputFileConstructor : InputFile
    {
        public long id;
        public int parts;
        public string name;
        public string md5Checksum;

        public InputFileConstructor()
        {

        }

        public InputFileConstructor(long id, int parts, string name, string md5Checksum)
        {
            this.id = id;
            this.parts = parts;
            this.name = name;
            this.md5Checksum = md5Checksum;
        }

        public override Constructor constructor => Constructor.InputFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf52ff27f);
            writer.Write(id);
            writer.Write(parts);
            Serializers.String.Write(writer, name);
            Serializers.String.Write(writer, md5Checksum);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            name = Serializers.String.Read(reader);
            md5Checksum = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(inputFile id:{id} parts:{parts} name:'{name}' md5Checksum:'{md5Checksum}')";
        }
    }

    public class InputMediaEmptyConstructor : InputMedia
    {
        public override Constructor constructor => Constructor.InputMediaEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9664f57f);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputMediaEmpty)";
        }
    }

    public class InputMediaUploadedPhotoConstructor : InputMedia
    {
        public InputFile file;

        public InputMediaUploadedPhotoConstructor()
        {

        }

        public InputMediaUploadedPhotoConstructor(InputFile file)
        {
            this.file = file;
        }

        public override Constructor constructor => Constructor.InputMediaUploadedPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2dc53a7d);
            file.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaUploadedPhoto file:{file})";
        }
    }

    public class InputMediaPhotoConstructor : InputMedia
    {
        public InputPhoto id;

        public InputMediaPhotoConstructor()
        {

        }

        public InputMediaPhotoConstructor(InputPhoto id)
        {
            this.id = id;
        }

        public override Constructor constructor => Constructor.InputMediaPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8f2ab2ec);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = Read<InputPhoto>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaPhoto id:{id})";
        }
    }

    public class InputMediaGeoPointConstructor : InputMedia
    {
        public InputGeoPoint geoPoint;

        public InputMediaGeoPointConstructor()
        {

        }

        public InputMediaGeoPointConstructor(InputGeoPoint geoPoint)
        {
            this.geoPoint = geoPoint;
        }

        public override Constructor constructor => Constructor.InputMediaGeoPoint;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf9c44144);
            geoPoint.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            geoPoint = Read<InputGeoPoint>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaGeoPoint geoPoint:{geoPoint})";
        }
    }

    public class InputMediaContactConstructor : InputMedia
    {
        public string phoneNumber;
        public string firstName;
        public string lastName;

        public InputMediaContactConstructor()
        {

        }

        public InputMediaContactConstructor(string phoneNumber, string firstName, string lastName)
        {
            this.phoneNumber = phoneNumber;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public override Constructor constructor => Constructor.InputMediaContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa6e45987);
            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
        }

        public override void Read(BinaryReader reader)
        {
            phoneNumber = Serializers.String.Read(reader);
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(inputMediaContact phoneNumber:'{phoneNumber}' firstName:'{firstName}' lastName:'{lastName}')";
        }
    }

    public class InputMediaUploadedVideoConstructor : InputMedia
    {
        public InputFile file;
        public int duration;
        public int w;
        public int h;

        public InputMediaUploadedVideoConstructor()
        {

        }

        public InputMediaUploadedVideoConstructor(InputFile file, int duration, int w, int h)
        {
            this.file = file;
            this.duration = duration;
            this.w = w;
            this.h = h;
        }

        public override Constructor constructor => Constructor.InputMediaUploadedVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4847d92a);
            file.Write(writer);
            writer.Write(duration);
            writer.Write(w);
            writer.Write(h);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
            duration = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputMediaUploadedVideo file:{file} duration:{duration} w:{w} h:{h})";
        }
    }

    public class InputMediaUploadedThumbVideoConstructor : InputMedia
    {
        public InputFile file;
        public InputFile thumb;
        public int duration;
        public int w;
        public int h;

        public InputMediaUploadedThumbVideoConstructor()
        {

        }

        public InputMediaUploadedThumbVideoConstructor(InputFile file, InputFile thumb, int duration, int w, int h)
        {
            this.file = file;
            this.thumb = thumb;
            this.duration = duration;
            this.w = w;
            this.h = h;
        }


        public override Constructor constructor => Constructor.InputMediaUploadedThumbVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe628a145);
            file.Write(writer);
            thumb.Write(writer);
            writer.Write(duration);
            writer.Write(w);
            writer.Write(h);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
            thumb = Read<InputFile>(reader);
            duration = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputMediaUploadedThumbVideo file:{file} thumb:{thumb} duration:{duration} w:{w} h:{h})";
        }
    }

    public class InputMediaVideoConstructor : InputMedia
    {
        public InputVideo id;

        public InputMediaVideoConstructor()
        {

        }

        public InputMediaVideoConstructor(InputVideo id)
        {
            this.id = id;
        }

        public override Constructor constructor => Constructor.InputMediaVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7f023ae6);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = Read<InputVideo>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaVideo id:{id})";
        }
    }

    public class InputChatPhotoEmptyConstructor : InputChatPhoto
    {
        public override Constructor constructor => Constructor.InputChatPhotoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1ca48f57);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputChatPhotoEmpty)";
        }
    }

    public class InputChatUploadedPhotoConstructor : InputChatPhoto
    {
        public InputFile file;
        public InputPhotoCrop crop;

        public InputChatUploadedPhotoConstructor()
        {

        }

        public InputChatUploadedPhotoConstructor(InputFile file, InputPhotoCrop crop)
        {
            this.file = file;
            this.crop = crop;
        }

        public override Constructor constructor => Constructor.InputChatUploadedPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x94254732);
            file.Write(writer);
            crop.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
            crop = Read<InputPhotoCrop>(reader);
        }

        public override string ToString()
        {
            return $"(inputChatUploadedPhoto file:{file} crop:{crop})";
        }
    }

    public class InputChatPhotoConstructor : InputChatPhoto
    {
        public InputPhoto id;
        public InputPhotoCrop crop;

        public InputChatPhotoConstructor()
        {

        }

        public InputChatPhotoConstructor(InputPhoto id, InputPhotoCrop crop)
        {
            this.id = id;
            this.crop = crop;
        }

        public override Constructor constructor => Constructor.InputChatPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb2e1bf08);
            id.Write(writer);
            crop.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = Read<InputPhoto>(reader);
            crop = Read<InputPhotoCrop>(reader);
        }

        public override string ToString()
        {
            return $"(inputChatPhoto id:{id} crop:{crop})";
        }
    }


    public class InputGeoPointEmptyConstructor : InputGeoPoint
    {
        public override Constructor constructor => Constructor.InputGeoPointEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe4c123d6);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputGeoPointEmpty)";
        }
    }


    public class InputGeoPointConstructor : InputGeoPoint
    {
        public double lat;
        public double lng;

        public InputGeoPointConstructor()
        {

        }

        public InputGeoPointConstructor(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }


        public override Constructor constructor => Constructor.InputGeoPoint;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf3b7acc9);
            writer.Write(lat);
            writer.Write(lng);
        }

        public override void Read(BinaryReader reader)
        {
            lat = reader.ReadDouble();
            lng = reader.ReadDouble();
        }

        public override string ToString()
        {
            return $"(inputGeoPoint lat:{lat} long:{lng})";
        }
    }


    public class InputPhotoEmptyConstructor : InputPhoto
    {
        public override Constructor constructor => Constructor.InputPhotoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1cd7bf0d);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputPhotoEmpty)";
        }
    }


    public class InputPhotoConstructor : InputPhoto
    {
        public long id;
        public long accessHash;

        public InputPhotoConstructor()
        {

        }

        public InputPhotoConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfb95c6c4);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputPhoto id:{id} accessHash:{accessHash})";
        }
    }


    public class InputVideoEmptyConstructor : InputVideo
    {
        public override Constructor constructor => Constructor.InputVideoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5508ec75);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputVideoEmpty)";
        }
    }


    public class InputVideoConstructor : InputVideo
    {
        public long id;
        public long accessHash;

        public InputVideoConstructor()
        {

        }

        public InputVideoConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xee579652);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputVideo id:{id} accessHash:{accessHash})";
        }
    }


    public class InputFileLocationConstructor : InputFileLocation
    {
        public long volumeId;
        public int localId;
        public long secret;

        public InputFileLocationConstructor()
        {

        }

        public InputFileLocationConstructor(long volumeId, int localId, long secret)
        {
            this.volumeId = volumeId;
            this.localId = localId;
            this.secret = secret;
        }


        public override Constructor constructor => Constructor.InputFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x14637196);
            writer.Write(volumeId);
            writer.Write(localId);
            writer.Write(secret);
        }

        public override void Read(BinaryReader reader)
        {
            volumeId = reader.ReadInt64();
            localId = reader.ReadInt32();
            secret = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputFileLocation volume_id:{volumeId} local_id:{localId} secret:{secret})";
        }
    }


    public class InputVideoFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long accessHash;

        public InputVideoFileLocationConstructor()
        {

        }

        public InputVideoFileLocationConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputVideoFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3d0364ec);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputVideoFileLocation id:{id} accessHash:{accessHash})";
        }
    }


    public class InputPhotoCropAutoConstructor : InputPhotoCrop
    {
        public override Constructor constructor => Constructor.InputPhotoCropAuto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xade6b004);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputPhotoCropAuto)";
        }
    }


    public class InputPhotoCropConstructor : InputPhotoCrop
    {
        public double cropLeft;
        public double cropTop;
        public double cropWidth;

        public InputPhotoCropConstructor()
        {

        }

        public InputPhotoCropConstructor(double cropLeft, double cropTop, double cropWidth)
        {
            this.cropLeft = cropLeft;
            this.cropTop = cropTop;
            this.cropWidth = cropWidth;
        }


        public override Constructor constructor => Constructor.InputPhotoCrop;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd9915325);
            writer.Write(cropLeft);
            writer.Write(cropTop);
            writer.Write(cropWidth);
        }

        public override void Read(BinaryReader reader)
        {
            cropLeft = reader.ReadDouble();
            cropTop = reader.ReadDouble();
            cropWidth = reader.ReadDouble();
        }

        public override string ToString()
        {
            return $"(inputPhotoCrop crop_left:{cropLeft} crop_top:{cropTop} crop_width:{cropWidth})";
        }
    }


    public class InputAppEventConstructor : InputAppEvent
    {
        public double time;
        public string type;
        public long peer;
        public string data;

        public InputAppEventConstructor()
        {

        }

        public InputAppEventConstructor(double time, string type, long peer, string data)
        {
            this.time = time;
            this.type = type;
            this.peer = peer;
            this.data = data;
        }


        public override Constructor constructor => Constructor.InputAppEvent;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x770656a8);
            writer.Write(time);
            Serializers.String.Write(writer, type);
            writer.Write(peer);
            Serializers.String.Write(writer, data);
        }

        public override void Read(BinaryReader reader)
        {
            time = reader.ReadDouble();
            type = Serializers.String.Read(reader);
            peer = reader.ReadInt64();
            data = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(inputAppEvent time:{time} type:'{type}' peer:{peer} data:'{data}')";
        }
    }


    public class PeerUserConstructor : Peer
    {
        public int userId;

        public PeerUserConstructor()
        {

        }

        public PeerUserConstructor(int userId)
        {
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.PeerUser;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9db1bc6d);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(peerUser userId:{userId})";
        }
    }


    public class PeerChatConstructor : Peer
    {
        public int chatId;

        public PeerChatConstructor()
        {

        }

        public PeerChatConstructor(int chatId)
        {
            this.chatId = chatId;
        }


        public override Constructor constructor => Constructor.PeerChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xbad0e5bb);
            writer.Write(chatId);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(peerChat chatId:{chatId})";
        }
    }


    public class StorageFileUnknownConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileUnknown;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xaa963b05);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileUnknown)";
        }
    }


    public class StorageFileJpegConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileJpeg;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x007efe0e);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileJpeg)";
        }
    }


    public class StorageFileGifConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileGif;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xcae1aadf);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileGif)";
        }
    }


    public class StorageFilePngConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFilePng;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0a4f63c0);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_filePng)";
        }
    }


    public class StorageFileMp3Constructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileMp3;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x528a0677);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileMp3)";
        }
    }


    public class StorageFileMovConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileMov;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4b09ebbc);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileMov)";
        }
    }


    public class StorageFilePartialConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFilePartial;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x40bc6f52);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_filePartial)";
        }
    }


    public class StorageFileMp4Constructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileMp4;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb3cea0e4);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileMp4)";
        }
    }


    public class StorageFileWebpConstructor : StorageFileType
    {
        public override Constructor constructor => Constructor.StorageFileWebp;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1081464c);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(storage_fileWebp)";
        }
    }


    public class FileLocationUnavailableConstructor : FileLocation
    {
        public long volumeId;
        public int localId;
        public long secret;

        public FileLocationUnavailableConstructor()
        {

        }

        public FileLocationUnavailableConstructor(long volumeId, int localId, long secret)
        {
            this.volumeId = volumeId;
            this.localId = localId;
            this.secret = secret;
        }


        public override Constructor constructor => Constructor.FileLocationUnavailable;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7c596b46);
            writer.Write(volumeId);
            writer.Write(localId);
            writer.Write(secret);
        }

        public override void Read(BinaryReader reader)
        {
            volumeId = reader.ReadInt64();
            localId = reader.ReadInt32();
            secret = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(fileLocationUnavailable volume_id:{volumeId} local_id:{localId} secret:{secret})";
        }
    }


    public class FileLocationConstructor : FileLocation
    {
        public int dcId;
        public long volumeId;
        public int localId;
        public long secret;

        public FileLocationConstructor()
        {

        }

        public FileLocationConstructor(int dcId, long volumeId, int localId, long secret)
        {
            this.dcId = dcId;
            this.volumeId = volumeId;
            this.localId = localId;
            this.secret = secret;
        }


        public override Constructor constructor => Constructor.FileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x53d69076);
            writer.Write(dcId);
            writer.Write(volumeId);
            writer.Write(localId);
            writer.Write(secret);
        }

        public override void Read(BinaryReader reader)
        {
            dcId = reader.ReadInt32();
            volumeId = reader.ReadInt64();
            localId = reader.ReadInt32();
            secret = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(fileLocation dc_id:{dcId} volume_id:{volumeId} local_id:{localId} secret:{secret})";
        }
    }


    public class UserEmptyConstructor : User
    {
        public int id;

        public UserEmptyConstructor()
        {

        }

        public UserEmptyConstructor(int id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.UserEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x200250ba);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(userEmpty id:{id})";
        }
    }


    public class UserSelfConstructor : User //userSelf#7007b451 id:int first_name:string last_name:string username:string phone:string photo:UserProfilePhoto status:UserStatus inactive:Bool = User;
    {
        public int id;
        public string firstName;
        public string lastName;
        public string username;
        public string phone;
        public UserProfilePhoto photo;
        public UserStatus status;
        public bool inactive;

        public UserSelfConstructor()
        {

        }

        public UserSelfConstructor(int id, string firstName, string lastName, string username, string phone, UserProfilePhoto photo,
            UserStatus status, bool inactive)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.phone = phone;
            this.photo = photo;
            this.status = status;
            this.inactive = inactive;
        }


        public override Constructor constructor => Constructor.UserSelf;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7007b451);
            writer.Write(id);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            Serializers.String.Write(writer, username);
            Serializers.String.Write(writer, phone);
            photo.Write(writer);
            status.Write(writer);
            writer.Write(inactive ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            phone = Serializers.String.Read(reader);
            photo = Read<UserProfilePhoto>(reader);
            status = Read<UserStatus>(reader);
            inactive = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return
                $"(userSelf id:{id} firstName:'{firstName}' lastName:'{lastName}' username: '{username}' phone:'{phone}' photo:{photo} status:{status} inactive:{inactive})";
        }
    }


    public class UserContactConstructor : User //userContact#cab35e18 id:int first_name:string last_name:string username:string access_hash:long phone:string photo:UserProfilePhoto status:UserStatus = User;
    {
        public int id;
        public string firstName;
        public string lastName;
        public string username;
        public long accessHash;
        public string phone;
        public UserProfilePhoto photo;
        public UserStatus status;

        public UserContactConstructor()
        {

        }

        public UserContactConstructor(int id, string firstName, string lastName, string username, long accessHash, string phone,
            UserProfilePhoto photo, UserStatus status)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.accessHash = accessHash;
            this.phone = phone;
            this.photo = photo;
            this.status = status;
        }


        public override Constructor constructor => Constructor.UserContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xcab35e18);
            writer.Write(id);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            Serializers.String.Write(writer, username);
            writer.Write(accessHash);
            Serializers.String.Write(writer, phone);
            photo.Write(writer);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            accessHash = reader.ReadInt64();
            phone = Serializers.String.Read(reader);
            photo = Read<UserProfilePhoto>(reader);
            status = Read<UserStatus>(reader);
        }

        public override string ToString()
        {
            return
                $"(userContact id:{id} firstName:'{firstName}' lastName:'{lastName}' username: '{username}' accessHash:{accessHash} phone:'{phone}' photo:{photo} status:{status})";
        }
    }


    public class UserRequestConstructor : User //userRequest#d9ccc4ef id:int first_name:string last_name:string username:string access_hash:long phone:string photo:UserProfilePhoto status:UserStatus = User;
    {
        public int id;
        public string firstName;
        public string lastName;
        public string username;
        public long accessHash;
        public string phone;
        public UserProfilePhoto photo;
        public UserStatus status;

        public UserRequestConstructor()
        {

        }

        public UserRequestConstructor(int id, string firstName, string lastName, string username, long accessHash, string phone,
            UserProfilePhoto photo, UserStatus status)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.accessHash = accessHash;
            this.phone = phone;
            this.photo = photo;
            this.status = status;
        }


        public override Constructor constructor => Constructor.UserRequest;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd9ccc4ef);
            writer.Write(id);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            Serializers.String.Write(writer, username);
            writer.Write(accessHash);
            Serializers.String.Write(writer, phone);
            photo.Write(writer);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            accessHash = reader.ReadInt64();
            phone = Serializers.String.Read(reader);
            photo = Read<UserProfilePhoto>(reader);
            status = Read<UserStatus>(reader);
        }

        public override string ToString()
        {
            return
                $"(userRequest id:{id} firstName:'{firstName}' lastName:'{lastName}' username:'{username}' accessHash:{accessHash} phone:'{phone}' photo:{photo} status:{status})";
        }
    }


    public class UserForeignConstructor : User //userForeign#75cf7a8 id:int first_name:string last_name:string username:string access_hash:long photo:UserProfilePhoto status:UserStatus = User;
    {
        public int id;
        public string firstName;
        public string lastName;
        public string username;
        public long accessHash;
        public UserProfilePhoto photo;
        public UserStatus status;

        public UserForeignConstructor()
        {

        }

        public UserForeignConstructor(int id, string firstName, string lastName, string username, long accessHash, UserProfilePhoto photo, UserStatus status)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.accessHash = accessHash;
            this.photo = photo;
            this.status = status;
        }

        public override Constructor constructor => Constructor.UserForeign;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x075cf7a8);
            writer.Write(id);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            Serializers.String.Write(writer, username);
            writer.Write(accessHash);
            photo.Write(writer);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            accessHash = reader.ReadInt64();
            photo = Read<UserProfilePhoto>(reader);
            status = Read<UserStatus>(reader);
        }

        public override string ToString()
        {
            return
                $"(userForeign id:{id} firstName:'{firstName}' lastName:'{lastName}' username:'{username}' accessHash:{accessHash} photo:{photo} status:{status})";
        }
    }

    public class UserDeletedConstructor : User //userDeleted#d6016d7a id:int first_name:string last_name:string username:string = User;
    {
        public int id;
        public string firstName;
        public string lastName;
        public string username;

        public UserDeletedConstructor()
        {

        }

        public UserDeletedConstructor(int id, string firstName, string lastName, string username)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
        }


        public override Constructor constructor => Constructor.UserDeleted;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd6016d7a);
            writer.Write(id);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            Serializers.String.Write(writer, username);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(userDeleted id:{id} firstName:'{firstName}' lastName:'{lastName}' username:'{username}')";
        }
    }


    public class UserProfilePhotoEmptyConstructor : UserProfilePhoto
    {
        public override Constructor constructor => Constructor.UserProfilePhotoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4f11bae1);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(userProfilePhotoEmpty)";
        }
    }


    public class UserProfilePhotoConstructor : UserProfilePhoto
    {
        public long photoId;
        public FileLocation photoSmall;
        public FileLocation photoBig;

        public UserProfilePhotoConstructor()
        {

        }

        public UserProfilePhotoConstructor(long photoId, FileLocation photoSmall, FileLocation photoBig)
        {
            this.photoId = photoId;
            this.photoSmall = photoSmall;
            this.photoBig = photoBig;
        }


        public override Constructor constructor => Constructor.UserProfilePhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd559d8c8);
            writer.Write(photoId);
            photoSmall.Write(writer);
            photoBig.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photoId = reader.ReadInt64();
            photoSmall = Read<FileLocation>(reader);
            photoBig = Read<FileLocation>(reader);
        }

        public override string ToString()
        {
            return $"(userProfilePhoto photoId:{photoId} photoSmall:{photoSmall} photoBig:{photoBig})";
        }
    }


    public class UserStatusEmptyConstructor : UserStatus
    {
        public override Constructor constructor => Constructor.UserStatusEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x09d05049);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(userStatusEmpty)";
        }
    }


    public class UserStatusOnlineConstructor : UserStatus
    {
        public int expires;

        public UserStatusOnlineConstructor()
        {

        }

        public UserStatusOnlineConstructor(int expires)
        {
            this.expires = expires;
        }


        public override Constructor constructor => Constructor.UserStatusOnline;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xedb93949);
            writer.Write(expires);
        }

        public override void Read(BinaryReader reader)
        {
            expires = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(userStatusOnline expires:{expires})";
        }
    }


    public class UserStatusOfflineConstructor : UserStatus
    {
        public int wasOnline;

        public UserStatusOfflineConstructor()
        {

        }

        public UserStatusOfflineConstructor(int wasOnline)
        {
            this.wasOnline = wasOnline;
        }


        public override Constructor constructor => Constructor.UserStatusOffline;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x008c703f);
            writer.Write(wasOnline);
        }

        public override void Read(BinaryReader reader)
        {
            wasOnline = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(userStatusOffline wasOnline:{wasOnline})";
        }
    }

    public class UserStatusRecentlyConstructor : UserStatus
    {
        public override Constructor constructor => Constructor.UserStatusRecently;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe26f42f1);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(userStatusRecently)";
        }
    }

    public class UserStatusLastWeekConstructor : UserStatus
    {
        public override Constructor constructor => Constructor.UserStatusLastWeek;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x07bf09fc);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(userStatusLastWeek)";
        }
    }

    public class UserStatusLastMonthConstructor : UserStatus
    {
        public override Constructor constructor => Constructor.UserStatusLastMonth;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x77ebc742);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(userStatusLastMonth)";
        }
    }

    public class ChatEmptyConstructor : Chat
    {
        public int id;

        public ChatEmptyConstructor()
        {

        }

        public ChatEmptyConstructor(int id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.ChatEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9ba2d800);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatEmpty id:{id})";
        }
    }


    public class ChatConstructor : Chat
    {
        public int id;
        public string title;
        public ChatPhoto photo;
        public int participantsCount;
        public int date;
        public bool left;
        public int version;

        public ChatConstructor()
        {

        }

        public ChatConstructor(int id, string title, ChatPhoto photo, int participantsCount, int date, bool left, int version)
        {
            this.id = id;
            this.title = title;
            this.photo = photo;
            this.participantsCount = participantsCount;
            this.date = date;
            this.left = left;
            this.version = version;
        }


        public override Constructor constructor => Constructor.Chat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6e9c9bc7);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            photo.Write(writer);
            writer.Write(participantsCount);
            writer.Write(date);
            writer.Write(left ? 0x997275b5 : 0xbc799737);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            photo = Read<ChatPhoto>(reader);
            participantsCount = reader.ReadInt32();
            date = reader.ReadInt32();
            left = reader.ReadUInt32() == 0x997275b5;
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(chat id:{id} title:'{title}' photo:{photo} participantsCount:{participantsCount} date:{date} left:{left} version:{version})";
        }
    }


    public class ChatForbiddenConstructor : Chat
    {
        public int id;
        public string title;
        public int date;

        public ChatForbiddenConstructor()
        {

        }

        public ChatForbiddenConstructor(int id, string title, int date)
        {
            this.id = id;
            this.title = title;
            this.date = date;
        }


        public override Constructor constructor => Constructor.ChatForbidden;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfb0ccc41);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatForbidden id:{id} title:'{title}' date:{date})";
        }
    }


    public class ChatFullConstructor : ChatFull
    {
        public int id;
        public ChatParticipants participants;
        public Photo chatPhoto;
        public PeerNotifySettings notifySettings;

        public ChatFullConstructor()
        {

        }

        public ChatFullConstructor(int id, ChatParticipants participants, Photo chatPhoto, PeerNotifySettings notifySettings)
        {
            this.id = id;
            this.participants = participants;
            this.chatPhoto = chatPhoto;
            this.notifySettings = notifySettings;
        }


        public override Constructor constructor => Constructor.ChatFull;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x630e61be);
            writer.Write(id);
            participants.Write(writer);
            chatPhoto.Write(writer);
            notifySettings.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            participants = Read<ChatParticipants>(reader);
            chatPhoto = Read<Photo>(reader);
            notifySettings = Read<PeerNotifySettings>(reader);
        }

        public override string ToString()
        {
            return
                $"(chatFull id:{id} participants:{participants} chatPhoto:{chatPhoto} notifySettings:{notifySettings})";
        }
    }


    public class ChatParticipantConstructor : ChatParticipant
    {
        public int userId;
        public int inviterId;
        public int date;

        public ChatParticipantConstructor()
        {

        }

        public ChatParticipantConstructor(int userId, int inviterId, int date)
        {
            this.userId = userId;
            this.inviterId = inviterId;
            this.date = date;
        }


        public override Constructor constructor => Constructor.ChatParticipant;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc8d7493e);
            writer.Write(userId);
            writer.Write(inviterId);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            inviterId = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatParticipant userId:{userId} inviterId:{inviterId} date:{date})";
        }
    }


    public class ChatParticipantsForbiddenConstructor : ChatParticipants
    {
        public int chatId;

        public ChatParticipantsForbiddenConstructor()
        {

        }

        public ChatParticipantsForbiddenConstructor(int chatId)
        {
            this.chatId = chatId;
        }


        public override Constructor constructor => Constructor.ChatParticipantsForbidden;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0fd2bb8a);
            writer.Write(chatId);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatParticipantsForbidden chatId:{chatId})";
        }
    }


    public class ChatParticipantsConstructor : ChatParticipants
    {
        public int chatId;
        public int adminId;
        public List<ChatParticipant> participants;
        public int version;

        public ChatParticipantsConstructor()
        {

        }

        public ChatParticipantsConstructor(int chatId, int adminId, List<ChatParticipant> participants, int version)
        {
            this.chatId = chatId;
            this.adminId = adminId;
            this.participants = participants;
            this.version = version;
        }


        public override Constructor constructor => Constructor.ChatParticipants;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7841b415);
            writer.Write(chatId);
            writer.Write(adminId);
            writer.Write(0x1cb5c415);
            writer.Write(participants.Count);
            foreach (ChatParticipant participantsElement in participants)
            {
                participantsElement.Write(writer);
            }
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            adminId = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int participantsLen = reader.ReadInt32();
            participants = new List<ChatParticipant>(participantsLen);
            for (int participantsIndex = 0; participantsIndex < participantsLen; participantsIndex++)
            {
                var participantsElement = Read<ChatParticipant>(reader);
                participants.Add(participantsElement);
            }
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(chatParticipants chatId:{chatId} admin_id:{adminId} participants:{Serializers.VectorToString(participants)} version:{version})";
        }
    }


    public class ChatPhotoEmptyConstructor : ChatPhoto
    {
        public override Constructor constructor => Constructor.ChatPhotoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x37c1011c);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(chatPhotoEmpty)";
        }
    }


    public class ChatPhotoConstructor : ChatPhoto
    {
        public FileLocation photoSmall;
        public FileLocation photoBig;

        public ChatPhotoConstructor()
        {

        }

        public ChatPhotoConstructor(FileLocation photoSmall, FileLocation photoBig)
        {
            this.photoSmall = photoSmall;
            this.photoBig = photoBig;
        }


        public override Constructor constructor => Constructor.ChatPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6153276a);
            photoSmall.Write(writer);
            photoBig.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photoSmall = Read<FileLocation>(reader);
            photoBig = Read<FileLocation>(reader);
        }

        public override string ToString()
        {
            return $"(chatPhoto photoSmall:{photoSmall} photoBig:{photoBig})";
        }
    }


    public class MessageEmptyConstructor : Message
    {
        public int id;

        public MessageEmptyConstructor()
        {

        }

        public MessageEmptyConstructor(int id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.MessageEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x83e5de54);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messageEmpty id:{id})";
        }
    }


    public class MessageConstructor : Message
    {
        public int flags;
        public int id;
        public int fromId;
        public Peer toId;
        public int date;
        public string message;
        public MessageMedia media;

        public bool unread => (flags & 0x1) > 0;
        public bool sentByCurrentUser => (flags & 0x2) > 0;

        public MessageConstructor()
        {

        }

        public MessageConstructor(int flags, int id, int fromId, Peer toId, int date, string message, MessageMedia media)
        {
            this.flags = flags;
            this.id = id;
            this.fromId = fromId;
            this.toId = toId;
            this.date = date;
            this.message = message;
            this.media = media;
        }

        public override Constructor constructor => Constructor.Message;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x567699b3);
            writer.Write(flags);
            writer.Write(id);
            writer.Write(fromId);
            toId.Write(writer);
            writer.Write(date);
            Serializers.String.Write(writer, message);
            media.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {

            flags = reader.ReadInt32();
            id = reader.ReadInt32();
            fromId = reader.ReadInt32();
            toId = Read<Peer>(reader);
            date = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            media = Read<MessageMedia>(reader);
        }

        public override string ToString()
        {
            return $"(message) Flags: {flags}, Id: {id}, FromId: {fromId}, ToId: {toId}, Date: {date}, Message: {message}, Media: {media}, Unread: {unread}, SentByCurrentUser: {sentByCurrentUser}";
        }
    }

    public class MessageForwardedConstructor : Message //messageForwarded#a367e716 flags:int id:int fwd_from_id:int fwd_date:int from_id:int to_id:Peer date:int message:string media:MessageMedia = Message;
    {
        public int flags;
        public int id;
        public int fwdFromId;
        public int fwdDate;
        public int fromId;
        public Peer toId;
        public int date;
        public string message;
        public MessageMedia media;

        public bool unread => (flags & 0x1) > 0;
        public bool sentByCurrentUser => (flags & 0x2) > 0;

        public MessageForwardedConstructor()
        {

        }

        public MessageForwardedConstructor(int flags, int id, int fwdFromId, int fwdDate, int fromId, Peer toId, int date, string message, MessageMedia media)
        {
            this.flags = flags;
            this.id = id;
            this.fwdFromId = fwdFromId;
            this.fwdDate = fwdDate;
            this.fromId = fromId;
            this.toId = toId;
            this.date = date;
            this.message = message;
            this.media = media;
        }

        public override Constructor constructor => Constructor.MessageForwarded;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa367e716);
            writer.Write(flags);
            writer.Write(id);
            writer.Write(fwdFromId);
            writer.Write(fwdDate);
            writer.Write(fromId);
            toId.Write(writer);
            writer.Write(date);
            Serializers.String.Write(writer, message);
            media.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            flags = reader.ReadInt32();
            id = reader.ReadInt32();
            fwdFromId = reader.ReadInt32();
            fwdDate = reader.ReadInt32();
            fromId = reader.ReadInt32();
            toId = Read<Peer>(reader);
            date = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            media = Read<MessageMedia>(reader);
        }

        public override string ToString()
        {
            return $"(messageForwarded) Flags: {flags}, Id: {id}, FwdFromId: {fwdFromId}, FwdDate: {fwdDate}, FromId: {fromId}, ToId: {toId}, Date: {date}, Message: {message}, Media: {media}, Unread: {unread}, SentByCurrentUser: {sentByCurrentUser}";
        }
    }


    public class MessageServiceConstructor : Message // messageService#1d86f70e flags:int id:int from_id:int to_id:Peer date:int action:MessageAction = Message;
    {
        public int flags;
        public int id;
        public int fromId;
        public Peer toId;
        public int date;
        public MessageAction action;

        public bool unread => (flags & 0x1) > 0;
        public bool sentByCurrentUser => (flags & 0x2) > 0;

        public MessageServiceConstructor() { }

        public MessageServiceConstructor(int flags, int id, int fromId, Peer toId, int date, MessageAction action)
        {
            this.flags = flags;
            this.id = id;
            this.fromId = fromId;
            this.toId = toId;
            this.date = date;
            this.action = action;
        }

        public override Constructor constructor => Constructor.MessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1d86f70e);
            writer.Write(flags);
            writer.Write(id);
            writer.Write(fromId);
            toId.Write(writer);
            writer.Write(date);
            action.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            flags = reader.ReadInt32();
            id = reader.ReadInt32();
            fromId = reader.ReadInt32();
            toId = Read<Peer>(reader);
            date = reader.ReadInt32();
            action = Read<MessageAction>(reader);
        }

        public override string ToString()
        {
            return $"(messageService) Flags: {flags}, Id: {id}, FromId: {fromId}, ToId: {toId}, Date: {date}, Action: {action}, Unread: {unread}, SentByCurrentUser: {sentByCurrentUser}";
        }
    }


    public class MessageMediaEmptyConstructor : MessageMedia
    {
        public override Constructor constructor => Constructor.MessageMediaEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3ded6320);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(messageMediaEmpty)";
        }
    }


    public class MessageMediaPhotoConstructor : MessageMedia
    {
        public Photo photo;

        public MessageMediaPhotoConstructor()
        {

        }

        public MessageMediaPhotoConstructor(Photo photo)
        {
            this.photo = photo;
        }


        public override Constructor constructor => Constructor.MessageMediaPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc8c45a2a);
            photo.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photo = Read<Photo>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaPhoto photo:{photo})";
        }
    }


    public class MessageMediaVideoConstructor : MessageMedia
    {
        public Video video;

        public MessageMediaVideoConstructor()
        {

        }

        public MessageMediaVideoConstructor(Video video)
        {
            this.video = video;
        }


        public override Constructor constructor => Constructor.MessageMediaVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa2d24290);
            video.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            video = Read<Video>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaVideo video:{video})";
        }
    }


    public class MessageMediaGeoConstructor : MessageMedia
    {
        public GeoPoint geo;

        public MessageMediaGeoConstructor()
        {

        }

        public MessageMediaGeoConstructor(GeoPoint geo)
        {
            this.geo = geo;
        }


        public override Constructor constructor => Constructor.MessageMediaGeo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x56e0d474);
            geo.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            geo = Read<GeoPoint>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaGeo geo:{geo})";
        }
    }


    public class MessageMediaContactConstructor : MessageMedia
    {
        public string phoneNumber;
        public string firstName;
        public string lastName;
        public int userId;

        public MessageMediaContactConstructor()
        {

        }

        public MessageMediaContactConstructor(string phoneNumber, string firstName, string lastName, int userId)
        {
            this.phoneNumber = phoneNumber;
            this.firstName = firstName;
            this.lastName = lastName;
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.MessageMediaContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5e7d2f39);
            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            phoneNumber = Serializers.String.Read(reader);
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(messageMediaContact phoneNumber:'{phoneNumber}' firstName:'{firstName}' lastName:'{lastName}' userId:{userId})";
        }
    }


    public class MessageMediaUnsupportedConstructor : MessageMedia
    {
        public byte[] bytes;

        public MessageMediaUnsupportedConstructor()
        {

        }

        public MessageMediaUnsupportedConstructor(byte[] bytes)
        {
            this.bytes = bytes;
        }


        public override Constructor constructor => Constructor.MessageMediaUnsupported;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x29632a36);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            bytes = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaUnsupported bytes:{BitConverter.ToString(bytes)})";
        }
    }


    public class MessageActionEmptyConstructor : MessageAction
    {
        public override Constructor constructor => Constructor.MessageActionEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb6aef7b0);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(messageActionEmpty)";
        }
    }


    public class MessageActionChatCreateConstructor : MessageAction
    {
        public string title;
        public List<int> users;

        public MessageActionChatCreateConstructor()
        {

        }

        public MessageActionChatCreateConstructor(string title, List<int> users)
        {
            this.title = title;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessageActionChatCreate;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa6638b9a);
            Serializers.String.Write(writer, title);
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (int usersElement in users)
            {
                writer.Write(usersElement);
            }
        }

        public override void Read(BinaryReader reader)
        {
            title = Serializers.String.Read(reader);
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<int>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = reader.ReadInt32();
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return $"(messageActionChatCreate title:'{title}' users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessageActionChatEditTitleConstructor : MessageAction
    {
        public string title;

        public MessageActionChatEditTitleConstructor()
        {

        }

        public MessageActionChatEditTitleConstructor(string title)
        {
            this.title = title;
        }


        public override Constructor constructor => Constructor.MessageActionChatEditTitle;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb5a1ce5a);
            Serializers.String.Write(writer, title);
        }

        public override void Read(BinaryReader reader)
        {
            title = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(messageActionChatEditTitle title:'{title}')";
        }
    }


    public class MessageActionChatEditPhotoConstructor : MessageAction
    {
        public Photo photo;

        public MessageActionChatEditPhotoConstructor()
        {

        }

        public MessageActionChatEditPhotoConstructor(Photo photo)
        {
            this.photo = photo;
        }


        public override Constructor constructor => Constructor.MessageActionChatEditPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7fcb13a8);
            photo.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photo = Read<Photo>(reader);
        }

        public override string ToString()
        {
            return $"(messageActionChatEditPhoto photo:{photo})";
        }
    }


    public class MessageActionChatDeletePhotoConstructor : MessageAction
    {
        public override Constructor constructor => Constructor.MessageActionChatDeletePhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x95e3fbef);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(messageActionChatDeletePhoto)";
        }
    }


    public class MessageActionChatAddUserConstructor : MessageAction
    {
        public int userId;

        public MessageActionChatAddUserConstructor()
        {

        }

        public MessageActionChatAddUserConstructor(int userId)
        {
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.MessageActionChatAddUser;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5e3cfc4b);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messageActionChatAddUser userId:{userId})";
        }
    }


    public class MessageActionChatDeleteUserConstructor : MessageAction
    {
        public int userId;

        public MessageActionChatDeleteUserConstructor()
        {

        }

        public MessageActionChatDeleteUserConstructor(int userId)
        {
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.MessageActionChatDeleteUser;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb2ae9b0c);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messageActionChatDeleteUser userId:{userId})";
        }
    }

    public class DialogConstructor : Dialog
    {
        public Peer peer;
        public int topMessage;
        public int unreadCount;
        public PeerNotifySettings peerNotifySettings;

        public DialogConstructor()
        {

        }

        public DialogConstructor(Peer peer, int topMessage, int unreadCount, PeerNotifySettings peerNotifySettings)
        {
            this.peer = peer;
            this.topMessage = topMessage;
            this.unreadCount = unreadCount;
            this.peerNotifySettings = peerNotifySettings;
        }


        public override Constructor constructor => Constructor.Dialog;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xab3a99ac);
            peer.Write(writer);
            writer.Write(topMessage);
            writer.Write(unreadCount);
            peerNotifySettings.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            peer = Read<Peer>(reader);
            topMessage = reader.ReadInt32();
            unreadCount = reader.ReadInt32();
            peerNotifySettings = Read<PeerNotifySettings>(reader);
        }

        public override string ToString()
        {
            return $"(dialog peer:{peer} top_message:{topMessage} unread_count:{unreadCount})";
        }
    }


    public class PhotoEmptyConstructor : Photo
    {
        public long id;

        public PhotoEmptyConstructor()
        {

        }

        public PhotoEmptyConstructor(long id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.PhotoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2331b22d);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(photoEmpty id:{id})";
        }
    }


    public class PhotoConstructor : Photo
    {
        public long id;
        public long accessHash;
        public int userId;
        public int date;
        public string caption;
        public GeoPoint geo;
        public List<PhotoSize> sizes;

        public PhotoConstructor()
        {

        }

        public PhotoConstructor(long id, long accessHash, int userId, int date, string caption, GeoPoint geo,
            List<PhotoSize> sizes)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.userId = userId;
            this.date = date;
            this.caption = caption;
            this.geo = geo;
            this.sizes = sizes;
        }


        public override Constructor constructor => Constructor.Photo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x22b56751);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(userId);
            writer.Write(date);
            Serializers.String.Write(writer, caption);
            geo.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(sizes.Count);
            foreach (PhotoSize sizesElement in sizes)
            {
                sizesElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
            userId = reader.ReadInt32();
            date = reader.ReadInt32();
            caption = Serializers.String.Read(reader);
            geo = Read<GeoPoint>(reader);
            reader.ReadInt32(); // vector code
            int sizesLen = reader.ReadInt32();
            sizes = new List<PhotoSize>(sizesLen);
            for (int sizesIndex = 0; sizesIndex < sizesLen; sizesIndex++)
            {
                var sizesElement = Read<PhotoSize>(reader);
                sizes.Add(sizesElement);
            }
        }

        public override string ToString()
        {
            return
                $"(photo id:{id} accessHash:{accessHash} userId:{userId} date:{date} caption:'{caption}' geo:{geo} sizes:{Serializers.VectorToString(sizes)})";
        }
    }


    public class PhotoSizeEmptyConstructor : PhotoSize
    {
        public string type;

        public PhotoSizeEmptyConstructor()
        {

        }

        public PhotoSizeEmptyConstructor(string type)
        {
            this.type = type;
        }


        public override Constructor constructor => Constructor.PhotoSizeEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0e17e23c);
            Serializers.String.Write(writer, type);
        }

        public override void Read(BinaryReader reader)
        {
            type = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(photoSizeEmpty type:'{type}')";
        }
    }


    public class PhotoSizeConstructor : PhotoSize
    {
        public string type;
        public FileLocation location;
        public int w;
        public int h;
        public int size;

        public PhotoSizeConstructor()
        {

        }

        public PhotoSizeConstructor(string type, FileLocation location, int w, int h, int size)
        {
            this.type = type;
            this.location = location;
            this.w = w;
            this.h = h;
            this.size = size;
        }


        public override Constructor constructor => Constructor.PhotoSize;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x77bfb61b);
            Serializers.String.Write(writer, type);
            location.Write(writer);
            writer.Write(w);
            writer.Write(h);
            writer.Write(size);
        }

        public override void Read(BinaryReader reader)
        {
            type = Serializers.String.Read(reader);
            location = Read<FileLocation>(reader);
            w = reader.ReadInt32();
            h = reader.ReadInt32();
            size = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(photoSize type:'{type}' location:{location} w:{w} h:{h} size:{size})";
        }
    }


    public class PhotoCachedSizeConstructor : PhotoSize
    {
        public string type;
        public FileLocation location;
        public int w;
        public int h;
        public byte[] bytes;

        public PhotoCachedSizeConstructor()
        {

        }

        public PhotoCachedSizeConstructor(string type, FileLocation location, int w, int h, byte[] bytes)
        {
            this.type = type;
            this.location = location;
            this.w = w;
            this.h = h;
            this.bytes = bytes;
        }


        public override Constructor constructor => Constructor.PhotoCachedSize;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe9a734fa);
            Serializers.String.Write(writer, type);
            location.Write(writer);
            writer.Write(w);
            writer.Write(h);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            type = Serializers.String.Read(reader);
            location = Read<FileLocation>(reader);
            w = reader.ReadInt32();
            h = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(photoCachedSize type:'{type}' location:{location} w:{w} h:{h} bytes:{BitConverter.ToString(bytes)})";
        }
    }


    public class VideoEmptyConstructor : Video
    {
        public long id;

        public VideoEmptyConstructor()
        {

        }

        public VideoEmptyConstructor(long id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.VideoEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc10658a8);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(videoEmpty id:{id})";
        }
    }


    public class VideoConstructor : Video
    {
        public long id;
        public long accessHash;
        public int userId;
        public int date;
        public string caption;
        public int duration;
        public int size;
        public PhotoSize thumb;
        public int dcId;
        public int w;
        public int h;

        public VideoConstructor()
        {

        }

        public VideoConstructor(long id, long accessHash, int userId, int date, string caption, int duration, int size,
            PhotoSize thumb, int dcId, int w, int h)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.userId = userId;
            this.date = date;
            this.caption = caption;
            this.duration = duration;
            this.size = size;
            this.thumb = thumb;
            this.dcId = dcId;
            this.w = w;
            this.h = h;
        }


        public override Constructor constructor => Constructor.Video;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x388fa391);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(userId);
            writer.Write(date);
            Serializers.String.Write(writer, caption);
            writer.Write(duration);
            writer.Write(size);
            thumb.Write(writer);
            writer.Write(dcId);
            writer.Write(w);
            writer.Write(h);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
            userId = reader.ReadInt32();
            date = reader.ReadInt32();
            caption = Serializers.String.Read(reader);
            duration = reader.ReadInt32();
            size = reader.ReadInt32();
            thumb = Read<PhotoSize>(reader);
            dcId = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(video id:{id} accessHash:{accessHash} userId:{userId} date:{date} caption:'{caption}' duration:{duration} size:{size} thumb:{thumb} dc_id:{dcId} w:{w} h:{h})";
        }
    }


    public class GeoPointEmptyConstructor : GeoPoint
    {
        public override Constructor constructor => Constructor.GeoPointEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1117dd5f);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(geoPointEmpty)";
        }
    }


    public class GeoPointConstructor : GeoPoint
    {
        public double lng;
        public double lat;

        public GeoPointConstructor()
        {

        }

        public GeoPointConstructor(double lng, double lat)
        {
            this.lng = lng;
            this.lat = lat;
        }


        public override Constructor constructor => Constructor.GeoPoint;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2049d70c);
            writer.Write(lng);
            writer.Write(lat);
        }

        public override void Read(BinaryReader reader)
        {
            lng = reader.ReadDouble();
            lat = reader.ReadDouble();
        }

        public override string ToString()
        {
            return $"(geoPoint long:{lng} lat:{lat})";
        }
    }


    public class AuthCheckedPhoneConstructor : AuthCheckedPhone
    {
        public bool phoneRegistered;
        public bool phoneInvited;

        public AuthCheckedPhoneConstructor()
        {

        }

        public AuthCheckedPhoneConstructor(bool phoneRegistered, bool phoneInvited)
        {
            this.phoneRegistered = phoneRegistered;
            this.phoneInvited = phoneInvited;
        }


        public override Constructor constructor => Constructor.AuthCheckedPhone;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe300cc3b);
            writer.Write(phoneRegistered ? 0x997275b5 : 0xbc799737);
            writer.Write(phoneInvited ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            phoneRegistered = reader.ReadUInt32() == 0x997275b5;
            phoneInvited = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(auth_checkedPhone phone_registered:{phoneRegistered} phone_invited:{phoneInvited})";
        }
    }


    public class AuthSentCodeConstructor : AuthSentCode
    {
        public override Constructor constructor => Constructor.AuthSentCode;
    }

    public class AuthSentAppCodeConstructor : AuthSentCode
    {
        public override Constructor constructor => Constructor.AuthSentAppCode;
    }

    public class AuthAuthorizationConstructor : AuthAuthorization
    {
        public int expires;
        public User user;

        public AuthAuthorizationConstructor()
        {

        }

        public AuthAuthorizationConstructor(int expires, User user)
        {
            this.expires = expires;
            this.user = user;
        }


        public override Constructor constructor => Constructor.AuthAuthorization;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf6b673a4);
            writer.Write(expires);
            user.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            expires = reader.ReadInt32();
            user = Read<User>(reader);
        }

        public override string ToString()
        {
            return $"(auth_authorization expires:{expires} user:{user})";
        }
    }


    public class AuthExportedAuthorizationConstructor : AuthExportedAuthorization
    {
        public int id;
        public byte[] bytes;

        public AuthExportedAuthorizationConstructor()
        {

        }

        public AuthExportedAuthorizationConstructor(int id, byte[] bytes)
        {
            this.id = id;
            this.bytes = bytes;
        }

        public override Constructor constructor => Constructor.AuthExportedAuthorization;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xdf969c2d);
            writer.Write(id);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return $"(auth_exportedAuthorization id:{id} bytes:{BitConverter.ToString(bytes)})";
        }
    }


    public class InputNotifyPeerConstructor : InputNotifyPeer
    {
        public InputPeer peer;

        public InputNotifyPeerConstructor()
        {

        }

        public InputNotifyPeerConstructor(InputPeer peer)
        {
            this.peer = peer;
        }


        public override Constructor constructor => Constructor.InputNotifyPeer;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb8bc5b0c);
            peer.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            peer = Read<InputPeer>(reader);
        }

        public override string ToString()
        {
            return $"(inputNotifyPeer peer:{peer})";
        }
    }


    public class InputNotifyUsersConstructor : InputNotifyPeer
    {
        public override Constructor constructor => Constructor.InputNotifyUsers;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x193b4417);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputNotifyUsers)";
        }
    }


    public class InputNotifyChatsConstructor : InputNotifyPeer
    {
        public override Constructor constructor => Constructor.InputNotifyChats;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4a95e84e);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputNotifyChats)";
        }
    }


    public class InputNotifyAllConstructor : InputNotifyPeer
    {
        public override Constructor constructor => Constructor.InputNotifyAll;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa429b886);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputNotifyAll)";
        }
    }


    public class InputPeerNotifyEventsEmptyConstructor : InputPeerNotifyEvents
    {
        public override Constructor constructor => Constructor.InputPeerNotifyEventsEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf03064d8);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputPeerNotifyEventsEmpty)";
        }
    }


    public class InputPeerNotifyEventsAllConstructor : InputPeerNotifyEvents
    {
        public override Constructor constructor => Constructor.InputPeerNotifyEventsAll;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe86a2c74);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputPeerNotifyEventsAll)";
        }
    }


    public class InputPeerNotifySettingsConstructor : InputPeerNotifySettings
    {
        public int muteUntil;
        public string sound;
        public bool showPreviews;
        public int eventsMask;

        public InputPeerNotifySettingsConstructor()
        {

        }

        public InputPeerNotifySettingsConstructor(int muteUntil, string sound, bool showPreviews, int eventsMask)
        {
            this.muteUntil = muteUntil;
            this.sound = sound;
            this.showPreviews = showPreviews;
            this.eventsMask = eventsMask;
        }


        public override Constructor constructor => Constructor.InputPeerNotifySettings;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x46a2ce98);
            writer.Write(muteUntil);
            Serializers.String.Write(writer, sound);
            writer.Write(showPreviews ? 0x997275b5 : 0xbc799737);
            writer.Write(eventsMask);
        }

        public override void Read(BinaryReader reader)
        {
            muteUntil = reader.ReadInt32();
            sound = Serializers.String.Read(reader);
            showPreviews = reader.ReadUInt32() == 0x997275b5;
            eventsMask = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(inputPeerNotifySettings mute_until:{muteUntil} sound:'{sound}' show_previews:{showPreviews} events_mask:{eventsMask})";
        }
    }


    public class PeerNotifyEventsEmptyConstructor : PeerNotifyEvents
    {
        public override Constructor constructor => Constructor.PeerNotifyEventsEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xadd53cb3);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(peerNotifyEventsEmpty)";
        }
    }


    public class PeerNotifyEventsAllConstructor : PeerNotifyEvents
    {
        public override Constructor constructor => Constructor.PeerNotifyEventsAll;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6d1ded88);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(peerNotifyEventsAll)";
        }
    }


    public class PeerNotifySettingsEmptyConstructor : PeerNotifySettings
    {
        public override Constructor constructor => Constructor.PeerNotifySettingsEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x70a68512);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(peerNotifySettingsEmpty)";
        }
    }


    public class PeerNotifySettingsConstructor : PeerNotifySettings
    {
        public int muteUntil;
        public string sound;
        public bool showPreviews;
        public int eventsMask;

        public PeerNotifySettingsConstructor()
        {

        }

        public PeerNotifySettingsConstructor(int muteUntil, string sound, bool showPreviews, int eventsMask)
        {
            this.muteUntil = muteUntil;
            this.sound = sound;
            this.showPreviews = showPreviews;
            this.eventsMask = eventsMask;
        }


        public override Constructor constructor => Constructor.PeerNotifySettings;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8d5e11ee);
            writer.Write(muteUntil);
            Serializers.String.Write(writer, sound);
            writer.Write(showPreviews ? 0x997275b5 : 0xbc799737);
            writer.Write(eventsMask);
        }

        public override void Read(BinaryReader reader)
        {
            muteUntil = reader.ReadInt32();
            sound = Serializers.String.Read(reader);
            showPreviews = reader.ReadUInt32() == 0x997275b5;
            eventsMask = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(peerNotifySettings mute_until:{muteUntil} sound:'{sound}' show_previews:{showPreviews} events_mask:{eventsMask})";
        }
    }


    public class WallPaperConstructor : WallPaper
    {
        public int id;
        public string title;
        public List<PhotoSize> sizes;
        public int color;

        public WallPaperConstructor()
        {

        }

        public WallPaperConstructor(int id, string title, List<PhotoSize> sizes, int color)
        {
            this.id = id;
            this.title = title;
            this.sizes = sizes;
            this.color = color;
        }


        public override Constructor constructor => Constructor.WallPaper;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xccb03657);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            writer.Write(0x1cb5c415);
            writer.Write(sizes.Count);
            foreach (PhotoSize sizesElement in sizes)
            {
                sizesElement.Write(writer);
            }
            writer.Write(color);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            reader.ReadInt32(); // vector code
            int sizesLen = reader.ReadInt32();
            sizes = new List<PhotoSize>(sizesLen);
            for (int sizesIndex = 0; sizesIndex < sizesLen; sizesIndex++)
            {
                var sizesElement = Read<PhotoSize>(reader);
                sizes.Add(sizesElement);
            }
            color = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(wallPaper id:{id} title:'{title}' sizes:{Serializers.VectorToString(sizes)} color:{color})";
        }
    }


    public class UserFullConstructor : UserFull //userFull#771095da user:User link:contacts.Link profile_photo:Photo notify_settings:PeerNotifySettings blocked:Bool real_first_name:string real_last_name:string = UserFull;
    {
        public User user;
        public ContactsLink link;
        public Photo profilePhoto;
        public PeerNotifySettings notifySettings;
        public bool blocked;
        public string realFirstName;
        public string realLastName;

        public UserFullConstructor()
        {

        }

        public UserFullConstructor(User user, ContactsLink link, Photo profilePhoto, PeerNotifySettings notifySettings,
            bool blocked, string realFirstName, string realLastName)
        {
            this.user = user;
            this.link = link;
            this.profilePhoto = profilePhoto;
            this.notifySettings = notifySettings;
            this.blocked = blocked;
            this.realFirstName = realFirstName;
            this.realLastName = realLastName;
        }


        public override Constructor constructor => Constructor.UserFull;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x771095da);
            user.Write(writer);
            link.Write(writer);
            profilePhoto.Write(writer);
            notifySettings.Write(writer);
            writer.Write(blocked ? 0x997275b5 : 0xbc799737);
            Serializers.String.Write(writer, realFirstName);
            Serializers.String.Write(writer, realLastName);
        }

        public override void Read(BinaryReader reader)
        {
            if (reader.ReadUInt32() == 0x7007b451)
            {
                user = new UserSelfConstructor();
            }
            else
            {
                user = new UserRequestConstructor();
            }
            user.Read(reader);
            link = Read<ContactsLink>(reader);
            profilePhoto = Read<Photo>(reader);
            notifySettings = Read<PeerNotifySettings>(reader);
            blocked = reader.ReadUInt32() == 0x997275b5;
            realFirstName = Serializers.String.Read(reader);
            realLastName = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(userFull user:{user} link:{link} profile_photo:{profilePhoto} notify_settings:{notifySettings} blocked:{blocked} realFirstName:'{realFirstName}' realLastNames:'{realLastName}')";
        }
    }


    public class ContactConstructor : Contact
    {
        public int userId;
        public bool mutual;

        public ContactConstructor()
        {

        }

        public ContactConstructor(int userId, bool mutual)
        {
            this.userId = userId;
            this.mutual = mutual;
        }


        public override Constructor constructor => Constructor.Contact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf911c994);
            writer.Write(userId);
            writer.Write(mutual ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            mutual = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(contact userId:{userId} mutual:{mutual})";
        }
    }


    public class ImportedContactConstructor : ImportedContact
    {
        public int userId;
        public long clientId;

        public ImportedContactConstructor()
        {

        }

        public ImportedContactConstructor(int userId, long clientId)
        {
            this.userId = userId;
            this.clientId = clientId;
        }


        public override Constructor constructor => Constructor.ImportedContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd0028438);
            writer.Write(userId);
            writer.Write(clientId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            clientId = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(importedContact userId:{userId} clientId:{clientId})";
        }
    }


    public class ContactBlockedConstructor : ContactBlocked
    {
        public int userId;
        public int date;

        public ContactBlockedConstructor()
        {

        }

        public ContactBlockedConstructor(int userId, int date)
        {
            this.userId = userId;
            this.date = date;
        }


        public override Constructor constructor => Constructor.ContactBlocked;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x561bc879);
            writer.Write(userId);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactBlocked userId:{userId} date:{date})";
        }
    }


    public class ContactFoundConstructor : ContactFound
    {
        public int userId;

        public ContactFoundConstructor()
        {

        }

        public ContactFoundConstructor(int userId)
        {
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.ContactFound;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xea879f95);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactFound userId:{userId})";
        }
    }


    public class ContactSuggestedConstructor : ContactSuggested
    {
        public int userId;
        public int mutualContacts;

        public ContactSuggestedConstructor()
        {

        }

        public ContactSuggestedConstructor(int userId, int mutualContacts)
        {
            this.userId = userId;
            this.mutualContacts = mutualContacts;
        }


        public override Constructor constructor => Constructor.ContactSuggested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3de191a1);
            writer.Write(userId);
            writer.Write(mutualContacts);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            mutualContacts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactSuggested userId:{userId} mutual_contacts:{mutualContacts})";
        }
    }


    public class ContactStatusConstructor : ContactStatus
    {
        public int userId;
        public int expires;

        public ContactStatusConstructor()
        {

        }

        public ContactStatusConstructor(int userId, int expires)
        {
            this.userId = userId;
            this.expires = expires;
        }


        public override Constructor constructor => Constructor.ContactStatus;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xaa77b873);
            writer.Write(userId);
            writer.Write(expires);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            expires = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactStatus userId:{userId} expires:{expires})";
        }
    }


    public class ChatLocatedConstructor : ChatLocated
    {
        public int chatId;
        public int distance;

        public ChatLocatedConstructor()
        {

        }

        public ChatLocatedConstructor(int chatId, int distance)
        {
            this.chatId = chatId;
            this.distance = distance;
        }


        public override Constructor constructor => Constructor.ChatLocated;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3631cf4c);
            writer.Write(chatId);
            writer.Write(distance);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            distance = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatLocated chatId:{chatId} distance:{distance})";
        }
    }


    public class ContactsForeignLinkUnknownConstructor : ContactsForeignLink
    {
        public override Constructor constructor => Constructor.ContactsForeignLinkUnknown;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x133421f8);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(contacts_foreignLinkUnknown)";
        }
    }


    public class ContactsForeignLinkRequestedConstructor : ContactsForeignLink
    {
        public bool hasPhone;

        public ContactsForeignLinkRequestedConstructor()
        {

        }

        public ContactsForeignLinkRequestedConstructor(bool hasPhone)
        {
            this.hasPhone = hasPhone;
        }


        public override Constructor constructor => Constructor.ContactsForeignLinkRequested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa7801f47);
            writer.Write(hasPhone ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            hasPhone = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(contacts_foreignLinkRequested has_phone:{hasPhone})";
        }
    }


    public class ContactsForeignLinkMutualConstructor : ContactsForeignLink
    {
        public override Constructor constructor => Constructor.ContactsForeignLinkMutual;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1bea8ce1);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(contacts_foreignLinkMutual)";
        }
    }


    public class ContactsMyLinkEmptyConstructor : ContactsMyLink
    {
        public override Constructor constructor => Constructor.ContactsMyLinkEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd22a1c60);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(contacts_myLinkEmpty)";
        }
    }


    public class ContactsMyLinkRequestedConstructor : ContactsMyLink
    {
        public bool contact;

        public ContactsMyLinkRequestedConstructor()
        {

        }

        public ContactsMyLinkRequestedConstructor(bool contact)
        {
            this.contact = contact;
        }


        public override Constructor constructor => Constructor.ContactsMyLinkRequested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6c69efee);
            writer.Write(contact ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            contact = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(contacts_myLinkRequested contact:{contact})";
        }
    }


    public class ContactsMyLinkContactConstructor : ContactsMyLink
    {
        public override Constructor constructor => Constructor.ContactsMyLinkContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc240ebd9);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(contacts_myLinkContact)";
        }
    }


    public class ContactsLinkConstructor : ContactsLink
    {
        public ContactsMyLink myLink;
        public ContactsForeignLink foreignLink;
        public User user;

        public ContactsLinkConstructor()
        {

        }

        public ContactsLinkConstructor(ContactsMyLink myLink, ContactsForeignLink foreignLink, User user)
        {
            this.myLink = myLink;
            this.foreignLink = foreignLink;
            this.user = user;
        }


        public override Constructor constructor => Constructor.ContactsLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xeccea3f5);
            myLink.Write(writer);
            foreignLink.Write(writer);
            user.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            myLink = Read<ContactsMyLink>(reader);
            foreignLink = Read<ContactsForeignLink>(reader);
            user = Read<User>(reader);
        }

        public override string ToString()
        {
            return $"(contacts_link my_link:{myLink} foreign_link:{foreignLink} user:{user})";
        }
    }


    public class ContactsContactsConstructor : ContactsContacts
    {
        public List<Contact> contacts;
        public List<User> users;

        public ContactsContactsConstructor()
        {

        }

        public ContactsContactsConstructor(List<Contact> contacts, List<User> users)
        {
            this.contacts = contacts;
            this.users = users;
        }


        public override Constructor constructor => Constructor.ContactsContacts;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6f8b8cb2);
            writer.Write(0x1cb5c415);
            writer.Write(contacts.Count);
            foreach (Contact contactsElement in contacts)
            {
                contactsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int contactsLen = reader.ReadInt32();
            contacts = new List<Contact>(contactsLen);
            for (int contactsIndex = 0; contactsIndex < contactsLen; contactsIndex++)
            {
                var contactsElement = Read<Contact>(reader);
                contacts.Add(contactsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_contacts contacts:{Serializers.VectorToString(contacts)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class ContactsContactsNotModifiedConstructor : ContactsContacts
    {
        public override Constructor constructor => Constructor.ContactsContactsNotModified;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb74ba9d2);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(contacts_contactsNotModified)";
        }
    }


    public class ContactsImportedContactsConstructor : ContactsImportedContacts
    {
        public List<ImportedContact> importedContacts;
        public List<long> retryContacts;
        public List<User> users;

        public override Constructor constructor => Constructor.ContactsImportedContacts;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xad524315);
            WriteVector(writer, importedContacts);
            WriteVector(writer, retryContacts, writer.Write);
            WriteVector(writer, users);
        }

        public override void Read(BinaryReader reader)
        {
            importedContacts = ReadVector<ImportedContact>(reader);
            retryContacts = ReadVector(reader, reader.ReadInt64);
            users = ReadVector<User>(reader);
        }

        public override string ToString()
        {
            return $"({constructor}) ImportedContacts: {importedContacts}, RetryContacts: {retryContacts}, Users: {users}";
        }
    }


    public class ContactsBlockedConstructor : ContactsBlocked
    {
        public List<ContactBlocked> blocked;
        public List<User> users;

        public ContactsBlockedConstructor()
        {

        }

        public ContactsBlockedConstructor(List<ContactBlocked> blocked, List<User> users)
        {
            this.blocked = blocked;
            this.users = users;
        }


        public override Constructor constructor => Constructor.ContactsBlocked;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1c138d15);
            writer.Write(0x1cb5c415);
            writer.Write(blocked.Count);
            foreach (ContactBlocked blockedElement in blocked)
            {
                blockedElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int blockedLen = reader.ReadInt32();
            blocked = new List<ContactBlocked>(blockedLen);
            for (int blockedIndex = 0; blockedIndex < blockedLen; blockedIndex++)
            {
                var blockedElement = Read<ContactBlocked>(reader);
                blocked.Add(blockedElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_blocked blocked:{Serializers.VectorToString(blocked)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class ContactsBlockedSliceConstructor : ContactsBlocked
    {
        public int count;
        public List<ContactBlocked> blocked;
        public List<User> users;

        public ContactsBlockedSliceConstructor()
        {

        }

        public ContactsBlockedSliceConstructor(int count, List<ContactBlocked> blocked, List<User> users)
        {
            this.count = count;
            this.blocked = blocked;
            this.users = users;
        }


        public override Constructor constructor => Constructor.ContactsBlockedSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x900802a1);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(blocked.Count);
            foreach (ContactBlocked blockedElement in blocked)
            {
                blockedElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int blockedLen = reader.ReadInt32();
            blocked = new List<ContactBlocked>(blockedLen);
            for (int blockedIndex = 0; blockedIndex < blockedLen; blockedIndex++)
            {
                var blockedElement = Read<ContactBlocked>(reader);
                blocked.Add(blockedElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_blockedSlice count:{count} blocked:{Serializers.VectorToString(blocked)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class ContactsFoundConstructor : ContactsFound
    {
        public List<ContactFound> results;
        public List<User> users;

        public ContactsFoundConstructor()
        {

        }

        public ContactsFoundConstructor(List<ContactFound> results, List<User> users)
        {
            this.results = results;
            this.users = users;
        }


        public override Constructor constructor => Constructor.ContactsFound;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0566000e);
            writer.Write(0x1cb5c415);
            writer.Write(results.Count);
            foreach (ContactFound resultsElement in results)
            {
                resultsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int resultsLen = reader.ReadInt32();
            results = new List<ContactFound>(resultsLen);
            for (int resultsIndex = 0; resultsIndex < resultsLen; resultsIndex++)
            {
                var resultsElement = Read<ContactFound>(reader);
                results.Add(resultsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_found results:{Serializers.VectorToString(results)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class ContactsSuggestedConstructor : ContactsSuggested
    {
        public List<ContactSuggested> results;
        public List<User> users;

        public ContactsSuggestedConstructor()
        {

        }

        public ContactsSuggestedConstructor(List<ContactSuggested> results, List<User> users)
        {
            this.results = results;
            this.users = users;
        }


        public override Constructor constructor => Constructor.ContactsSuggested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5649dcc5);
            writer.Write(0x1cb5c415);
            writer.Write(results.Count);
            foreach (ContactSuggested resultsElement in results)
            {
                resultsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int resultsLen = reader.ReadInt32();
            results = new List<ContactSuggested>(resultsLen);
            for (int resultsIndex = 0; resultsIndex < resultsLen; resultsIndex++)
            {
                var resultsElement = Read<ContactSuggested>(reader);
                results.Add(resultsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_suggested results:{Serializers.VectorToString(results)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesDialogsConstructor : MessagesDialogs
    {
        public List<Dialog> dialogs;
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public MessagesDialogsConstructor()
        {

        }

        public MessagesDialogsConstructor(List<Dialog> dialogs, List<Message> messages, List<Chat> chats, List<User> users)
        {
            this.dialogs = dialogs;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesDialogs;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x15ba6c40);

            WriteVector(writer, dialogs);
            WriteVector(writer, messages);
            WriteVector(writer, chats);
            WriteVector(writer, users);
        }

        public override void Read(BinaryReader reader)
        {
            dialogs = ReadVector<Dialog>(reader);
            messages = ReadVector<Message>(reader);
            chats = ReadVector<Chat>(reader);
            users = ReadVector<User>(reader);
        }

        public override string ToString()
        {
            return
                $"(messages_dialogs dialogs:{Serializers.VectorToString(dialogs)} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesDialogsSliceConstructor : MessagesDialogs
    {
        public int count;
        public List<Dialog> dialogs;
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public MessagesDialogsSliceConstructor()
        {

        }

        public MessagesDialogsSliceConstructor(int count, List<Dialog> dialogs, List<Message> messages, List<Chat> chats,
            List<User> users)
        {
            this.count = count;
            this.dialogs = dialogs;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesDialogsSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x71e094f3);
            writer.Write(count);

            WriteVector(writer, dialogs);
            WriteVector(writer, messages);
            WriteVector(writer, chats);
            WriteVector(writer, users);
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();

            dialogs = ReadVector<Dialog>(reader);
            messages = ReadVector<Message>(reader);
            chats = ReadVector<Chat>(reader);
            users = ReadVector<User>(reader);
        }

        public override string ToString()
        {
            return
                $"(messages_dialogsSlice count:{count} dialogs:{Serializers.VectorToString(dialogs)} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesMessagesConstructor : MessagesMessages
    {
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public MessagesMessagesConstructor()
        {

        }

        public MessagesMessagesConstructor(List<Message> messages, List<Chat> chats, List<User> users)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8c718e87);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<Message>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<Message>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_messages messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesMessagesSliceConstructor : MessagesMessages
    {
        public int count;
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public MessagesMessagesSliceConstructor()
        {

        }

        public MessagesMessagesSliceConstructor(int count, List<Message> messages, List<Chat> chats, List<User> users)
        {
            this.count = count;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesMessagesSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0b446ae3);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<Message>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<Message>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_messagesSlice count:{count} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesMessageEmptyConstructor : MessagesMessage
    {
        public override Constructor constructor => Constructor.MessagesMessageEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3f4e0648);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(messages_messageEmpty)";
        }
    }


    public class MessagesMessageConstructor : MessagesMessage
    {
        public Message message;
        public List<Chat> chats;
        public List<User> users;

        public MessagesMessageConstructor()
        {

        }

        public MessagesMessageConstructor(Message message, List<Chat> chats, List<User> users)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xff90c417);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<Message>(reader);
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_message message:{message} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesStatedMessagesConstructor : MessagesStatedMessages
    {
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;
        public int pts;
        public int seq;

        public MessagesStatedMessagesConstructor()
        {

        }

        public MessagesStatedMessagesConstructor(List<Message> messages, List<Chat> chats, List<User> users, int pts, int seq)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.MessagesStatedMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x969478bb);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<Message>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<Message>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(messages_statedMessages messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} pts:{pts} seq:{seq})";
        }
    }


    public class MessagesStatedMessageConstructor : MessagesStatedMessage
    {
        public Message message;
        public List<Chat> chats;
        public List<User> users;
        public int pts;
        public int seq;

        public MessagesStatedMessageConstructor() { }

        public MessagesStatedMessageConstructor(Message message, List<Chat> chats, List<User> users, int pts, int seq)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
            this.pts = pts;
            this.seq = seq;
        }

        public override Constructor constructor => Constructor.MessagesStatedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd07ae726);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<Message>(reader);
            chats = ReadVector<Chat>(reader);
            users = ReadVector<User>(reader);

            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(messages_statedMessage message:{message} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} pts:{pts} seq:{seq})";
        }
    }


    public class SentMessageConstructor : SentMessage
    {
        public int id;
        public int date;
        public int pts;
        public int seq;

        public SentMessageConstructor()
        {

        }

        public SentMessageConstructor(int id, int date, int pts, int seq)
        {
            this.id = id;
            this.date = date;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.MessagesSentMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd1f4d35c);
            writer.Write(id);
            writer.Write(date);
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            date = reader.ReadInt32();
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messages_sentMessage id:{id} date:{date} pts:{pts} seq:{seq})";
        }
    }


    public class MessagesChatConstructor : MessagesChat
    {
        public Chat chat;
        public List<User> users;

        public MessagesChatConstructor()
        {

        }

        public MessagesChatConstructor(Chat chat, List<User> users)
        {
            this.chat = chat;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x40e9002a);
            chat.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            chat = Read<Chat>(reader);
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return $"(messages_chat chat:{chat} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesChatsConstructor : MessagesChats
    {
        public List<Chat> chats;
        public List<User> users;

        public MessagesChatsConstructor()
        {

        }

        public MessagesChatsConstructor(List<Chat> chats, List<User> users)
        {
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesChats;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8150cbd8);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_chats chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesChatFullConstructor : MessagesChatFull
    {
        public ChatFull fullChat;
        public List<Chat> chats;
        public List<User> users;

        public MessagesChatFullConstructor()
        {

        }

        public MessagesChatFullConstructor(ChatFull fullChat, List<Chat> chats, List<User> users)
        {
            this.fullChat = fullChat;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.MessagesChatFull;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe5d7d19c);
            fullChat.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            fullChat = Read<ChatFull>(reader);
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_chatFull full_chat:{fullChat} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessagesAffectedHistoryConstructor : MessagesAffectedHistory
    {
        public int pts;
        public int seq;
        public int offset;

        public MessagesAffectedHistoryConstructor()
        {

        }

        public MessagesAffectedHistoryConstructor(int pts, int seq, int offset)
        {
            this.pts = pts;
            this.seq = seq;
            this.offset = offset;
        }


        public override Constructor constructor => Constructor.MessagesAffectedHistory;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb7de36f2);
            writer.Write(pts);
            writer.Write(seq);
            writer.Write(offset);
        }

        public override void Read(BinaryReader reader)
        {
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
            offset = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messages_affectedHistory pts:{pts} seq:{seq} offset:{offset})";
        }
    }


    public class InputMessagesFilterEmptyConstructor : MessagesFilter
    {
        public override Constructor constructor => Constructor.InputMessagesFilterEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x57e2f66c);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputMessagesFilterEmpty)";
        }
    }


    public class InputMessagesFilterPhotosConstructor : MessagesFilter
    {
        public override Constructor constructor => Constructor.InputMessagesFilterPhotos;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9609a51c);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputMessagesFilterPhotos)";
        }
    }


    public class InputMessagesFilterVideoConstructor : MessagesFilter
    {
        public override Constructor constructor => Constructor.InputMessagesFilterVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9fc00e65);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputMessagesFilterVideo)";
        }
    }


    public class InputMessagesFilterPhotoVideoConstructor : MessagesFilter
    {
        public override Constructor constructor => Constructor.InputMessagesFilterPhotoVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x56e9f0e4);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputMessagesFilterPhotoVideo)";
        }
    }


    public class UpdateNewMessageConstructor : Update
    {
        public Message message;
        public int pts;

        public UpdateNewMessageConstructor()
        {

        }

        public UpdateNewMessageConstructor(Message message, int pts)
        {
            this.message = message;
            this.pts = pts;
        }


        public override Constructor constructor => Constructor.UpdateNewMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x013abdb3);
            message.Write(writer);
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<Message>(reader);
            pts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateNewMessage message:{message} pts:{pts})";
        }
    }


    public class UpdateMessageIdConstructor : Update
    {
        public int id;
        public long randomId;

        public UpdateMessageIdConstructor()
        {

        }

        public UpdateMessageIdConstructor(int id, long randomId)
        {
            this.id = id;
            this.randomId = randomId;
        }


        public override Constructor constructor => Constructor.UpdateMessageId;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4e90bfd6);
            writer.Write(id);
            writer.Write(randomId);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            randomId = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(updateMessageID id:{id} randomId:{randomId})";
        }
    }


    public class UpdateReadMessagesConstructor : Update
    {
        public List<int> messages;
        public int pts;

        public UpdateReadMessagesConstructor()
        {

        }

        public UpdateReadMessagesConstructor(List<int> messages, int pts)
        {
            this.messages = messages;
            this.pts = pts;
        }


        public override Constructor constructor => Constructor.UpdateReadMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc6649e31);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (int messagesElement in messages)
            {
                writer.Write(messagesElement);
            }
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<int>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = reader.ReadInt32();
                messages.Add(messagesElement);
            }
            pts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateReadMessages messages:{Serializers.VectorToString(messages)} pts:{pts})";
        }
    }


    public class UpdateDeleteMessagesConstructor : Update
    {
        public List<int> messages;
        public int pts;

        public UpdateDeleteMessagesConstructor()
        {

        }

        public UpdateDeleteMessagesConstructor(List<int> messages, int pts)
        {
            this.messages = messages;
            this.pts = pts;
        }


        public override Constructor constructor => Constructor.UpdateDeleteMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa92bfe26);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (int messagesElement in messages)
            {
                writer.Write(messagesElement);
            }
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<int>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = reader.ReadInt32();
                messages.Add(messagesElement);
            }
            pts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateDeleteMessages messages:{Serializers.VectorToString(messages)} pts:{pts})";
        }
    }


    public class UpdateRestoreMessagesConstructor : Update
    {
        public List<int> messages;
        public int pts;

        public UpdateRestoreMessagesConstructor()
        {

        }

        public UpdateRestoreMessagesConstructor(List<int> messages, int pts)
        {
            this.messages = messages;
            this.pts = pts;
        }


        public override Constructor constructor => Constructor.UpdateRestoreMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd15de04d);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (int messagesElement in messages)
            {
                writer.Write(messagesElement);
            }
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<int>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = reader.ReadInt32();
                messages.Add(messagesElement);
            }
            pts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateRestoreMessages messages:{Serializers.VectorToString(messages)} pts:{pts})";
        }
    }


    public class UpdateUserTypingConstructor : Update
    {
        public int userId;

        public UpdateUserTypingConstructor()
        {

        }

        public UpdateUserTypingConstructor(int userId)
        {
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.UpdateUserTyping;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5c486927);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateUserTyping userId:{userId})";
        }
    }


    public class UpdateChatUserTypingConstructor : Update
    {
        public int chatId;
        public int userId;

        public UpdateChatUserTypingConstructor()
        {

        }

        public UpdateChatUserTypingConstructor(int chatId, int userId)
        {
            this.chatId = chatId;
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.UpdateChatUserTyping;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9a65ea1f);
            writer.Write(chatId);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateChatUserTyping chatId:{chatId} userId:{userId})";
        }
    }


    public class UpdateChatParticipantsConstructor : Update
    {
        public ChatParticipants participants;

        public UpdateChatParticipantsConstructor()
        {

        }

        public UpdateChatParticipantsConstructor(ChatParticipants participants)
        {
            this.participants = participants;
        }


        public override Constructor constructor => Constructor.UpdateChatParticipants;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x07761198);
            participants.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            participants = Read<ChatParticipants>(reader);
        }

        public override string ToString()
        {
            return $"(updateChatParticipants participants:{participants})";
        }
    }


    public class UpdateUserStatusConstructor : Update
    {
        public int userId;
        public UserStatus status;

        public UpdateUserStatusConstructor()
        {

        }

        public UpdateUserStatusConstructor(int userId, UserStatus status)
        {
            this.userId = userId;
            this.status = status;
        }


        public override Constructor constructor => Constructor.UpdateUserStatus;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1bfbd823);
            writer.Write(userId);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            status = Read<UserStatus>(reader);
        }

        public override string ToString()
        {
            return $"(updateUserStatus userId:{userId} status:{status})";
        }
    }


    public class UpdateUserNameConstructor : Update
    {
        public int userId;
        public string firstName;
        public string lastName;

        public UpdateUserNameConstructor()
        {

        }

        public UpdateUserNameConstructor(int userId, string firstName, string lastName)
        {
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
        }


        public override Constructor constructor => Constructor.UpdateUserName;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa7332b73);
            writer.Write(userId);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(updateUserName userId:{userId} firstName:'{firstName}' lastName:'{lastName}')";
        }
    }


    public class UpdateUserPhotoConstructor : Update
    {
        public int userId;
        public int date;
        public UserProfilePhoto photo;
        public bool previous;

        public UpdateUserPhotoConstructor()
        {

        }

        public UpdateUserPhotoConstructor(int userId, int date, UserProfilePhoto photo, bool previous)
        {
            this.userId = userId;
            this.date = date;
            this.photo = photo;
            this.previous = previous;
        }


        public override Constructor constructor => Constructor.UpdateUserPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x95313b0c);
            writer.Write(userId);
            writer.Write(date);
            photo.Write(writer);
            writer.Write(previous ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            date = reader.ReadInt32();
            photo = Read<UserProfilePhoto>(reader);
            previous = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(updateUserPhoto userId:{userId} date:{date} photo:{photo} previous:{previous})";
        }
    }


    public class UpdateContactRegisteredConstructor : Update
    {
        public int userId;
        public int date;

        public UpdateContactRegisteredConstructor()
        {

        }

        public UpdateContactRegisteredConstructor(int userId, int date)
        {
            this.userId = userId;
            this.date = date;
        }


        public override Constructor constructor => Constructor.UpdateContactRegistered;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2575bbb9);
            writer.Write(userId);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateContactRegistered userId:{userId} date:{date})";
        }
    }


    public class UpdateContactLinkConstructor : Update
    {
        public int userId;
        public ContactsMyLink myLink;
        public ContactsForeignLink foreignLink;

        public UpdateContactLinkConstructor()
        {

        }

        public UpdateContactLinkConstructor(int userId, ContactsMyLink myLink, ContactsForeignLink foreignLink)
        {
            this.userId = userId;
            this.myLink = myLink;
            this.foreignLink = foreignLink;
        }


        public override Constructor constructor => Constructor.UpdateContactLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x51a48a9a);
            writer.Write(userId);
            myLink.Write(writer);
            foreignLink.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            myLink = Read<ContactsMyLink>(reader);
            foreignLink = Read<ContactsForeignLink>(reader);
        }

        public override string ToString()
        {
            return $"(updateContactLink userId:{userId} my_link:{myLink} foreign_link:{foreignLink})";
        }
    }


    public class UpdateActivationConstructor : Update
    {
        public int userId;

        public UpdateActivationConstructor()
        {

        }

        public UpdateActivationConstructor(int userId)
        {
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.UpdateActivation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6f690963);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateActivation userId:{userId})";
        }
    }


    public class UpdateNewAuthorizationConstructor : Update
    {
        public long authKeyId;
        public int date;
        public string device;
        public string location;

        public UpdateNewAuthorizationConstructor()
        {

        }

        public UpdateNewAuthorizationConstructor(long authKeyId, int date, string device, string location)
        {
            this.authKeyId = authKeyId;
            this.date = date;
            this.device = device;
            this.location = location;
        }


        public override Constructor constructor => Constructor.UpdateNewAuthorization;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8f06529a);
            writer.Write(authKeyId);
            writer.Write(date);
            Serializers.String.Write(writer, device);
            Serializers.String.Write(writer, location);
        }

        public override void Read(BinaryReader reader)
        {
            authKeyId = reader.ReadInt64();
            date = reader.ReadInt32();
            device = Serializers.String.Read(reader);
            location = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(updateNewAuthorization authKeyId:{authKeyId} date:{date} device:'{device}' location:'{location}')";
        }
    }


    public class UpdatesStateConstructor : UpdatesState
    {
        public int pts;
        public int qts;
        public int date;
        public int seq;
        public int unreadCount;

        public UpdatesStateConstructor()
        {

        }

        public UpdatesStateConstructor(int pts, int qts, int date, int seq, int unreadCount)
        {
            this.pts = pts;
            this.qts = qts;
            this.date = date;
            this.seq = seq;
            this.unreadCount = unreadCount;
        }


        public override Constructor constructor => Constructor.UpdatesState;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa56c2a3e);
            writer.Write(pts);
            writer.Write(qts);
            writer.Write(date);
            writer.Write(seq);
            writer.Write(unreadCount);
        }

        public override void Read(BinaryReader reader)
        {
            pts = reader.ReadInt32();
            qts = reader.ReadInt32();
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
            unreadCount = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updates_state pts:{pts} qts:{qts} date:{date} seq:{seq} unreadCount:{unreadCount})";
        }
    }


    public class UpdatesDifferenceEmptyConstructor : UpdatesDifference
    {
        public int date;
        public int seq;

        public UpdatesDifferenceEmptyConstructor()
        {

        }

        public UpdatesDifferenceEmptyConstructor(int date, int seq)
        {
            this.date = date;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.UpdatesDifferenceEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5d75a138);
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updates_differenceEmpty date:{date} seq:{seq})";
        }
    }


    public class UpdatesDifferenceConstructor : UpdatesDifference
    {
        public List<Message> newMessages;
        public List<EncryptedMessage> newEncryptedMessages;
        public List<Update> otherUpdates;
        public List<Chat> chats;
        public List<User> users;
        public UpdatesState state;

        public UpdatesDifferenceConstructor()
        {

        }

        public UpdatesDifferenceConstructor(List<Message> newMessages, List<EncryptedMessage> newEncryptedMessages,
            List<Update> otherUpdates, List<Chat> chats, List<User> users, UpdatesState state)
        {
            this.newMessages = newMessages;
            this.newEncryptedMessages = newEncryptedMessages;
            this.otherUpdates = otherUpdates;
            this.chats = chats;
            this.users = users;
            this.state = state;
        }


        public override Constructor constructor => Constructor.UpdatesDifference;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x00f49ca0);
            writer.Write(0x1cb5c415);
            writer.Write(newMessages.Count);
            foreach (Message newMessagesElement in newMessages)
            {
                newMessagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(newEncryptedMessages.Count);
            foreach (EncryptedMessage newEncryptedMessagesElement in newEncryptedMessages)
            {
                newEncryptedMessagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(otherUpdates.Count);
            foreach (Update otherUpdatesElement in otherUpdates)
            {
                otherUpdatesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            state.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int newMessagesLen = reader.ReadInt32();
            newMessages = new List<Message>(newMessagesLen);
            for (int newMessagesIndex = 0; newMessagesIndex < newMessagesLen; newMessagesIndex++)
            {
                var newMessagesElement = Read<Message>(reader);
                newMessages.Add(newMessagesElement);
            }
            reader.ReadInt32(); // vector code
            int newEncryptedMessagesLen = reader.ReadInt32();
            newEncryptedMessages = new List<EncryptedMessage>(newEncryptedMessagesLen);
            for (int newEncryptedMessagesIndex = 0;
                newEncryptedMessagesIndex < newEncryptedMessagesLen;
                newEncryptedMessagesIndex++)
            {
                var newEncryptedMessagesElement = Read<EncryptedMessage>(reader);
                newEncryptedMessages.Add(newEncryptedMessagesElement);
            }
            reader.ReadInt32(); // vector code
            int otherUpdatesLen = reader.ReadInt32();
            otherUpdates = new List<Update>(otherUpdatesLen);
            for (int otherUpdatesIndex = 0; otherUpdatesIndex < otherUpdatesLen; otherUpdatesIndex++)
            {
                var otherUpdatesElement = Read<Update>(reader);
                otherUpdates.Add(otherUpdatesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            state = Read<UpdatesState>(reader);
        }

        public override string ToString()
        {
            return
                $"(updates_difference newMessages:{Serializers.VectorToString(newMessages)} newEncryptedMessages:{Serializers.VectorToString(newEncryptedMessages)} other_updates:{Serializers.VectorToString(otherUpdates)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} state:{state})";
        }
    }


    public class UpdatesDifferenceSliceConstructor : UpdatesDifference
    {
        public List<Message> newMessages;
        public List<EncryptedMessage> newEncryptedMessages;
        public List<Update> otherUpdates;
        public List<Chat> chats;
        public List<User> users;
        public UpdatesState intermediateState;

        public UpdatesDifferenceSliceConstructor()
        {

        }

        public UpdatesDifferenceSliceConstructor(List<Message> newMessages, List<EncryptedMessage> newEncryptedMessages,
            List<Update> otherUpdates, List<Chat> chats, List<User> users, UpdatesState intermediateState)
        {
            this.newMessages = newMessages;
            this.newEncryptedMessages = newEncryptedMessages;
            this.otherUpdates = otherUpdates;
            this.chats = chats;
            this.users = users;
            this.intermediateState = intermediateState;
        }


        public override Constructor constructor => Constructor.UpdatesDifferenceSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa8fb1981);
            writer.Write(0x1cb5c415);
            writer.Write(newMessages.Count);
            foreach (Message newMessagesElement in newMessages)
            {
                newMessagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(newEncryptedMessages.Count);
            foreach (EncryptedMessage newEncryptedMessagesElement in newEncryptedMessages)
            {
                newEncryptedMessagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(otherUpdates.Count);
            foreach (Update otherUpdatesElement in otherUpdates)
            {
                otherUpdatesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            intermediateState.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int newMessagesLen = reader.ReadInt32();
            newMessages = new List<Message>(newMessagesLen);
            for (int newMessagesIndex = 0; newMessagesIndex < newMessagesLen; newMessagesIndex++)
            {
                var newMessagesElement = Read<Message>(reader);
                newMessages.Add(newMessagesElement);
            }
            reader.ReadInt32(); // vector code
            int newEncryptedMessagesLen = reader.ReadInt32();
            newEncryptedMessages = new List<EncryptedMessage>(newEncryptedMessagesLen);
            for (int newEncryptedMessagesIndex = 0;
                newEncryptedMessagesIndex < newEncryptedMessagesLen;
                newEncryptedMessagesIndex++)
            {
                var newEncryptedMessagesElement = Read<EncryptedMessage>(reader);
                newEncryptedMessages.Add(newEncryptedMessagesElement);
            }
            reader.ReadInt32(); // vector code
            int otherUpdatesLen = reader.ReadInt32();
            otherUpdates = new List<Update>(otherUpdatesLen);
            for (int otherUpdatesIndex = 0; otherUpdatesIndex < otherUpdatesLen; otherUpdatesIndex++)
            {
                var otherUpdatesElement = Read<Update>(reader);
                otherUpdates.Add(otherUpdatesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            intermediateState = Read<UpdatesState>(reader);
        }

        public override string ToString()
        {
            return
                $"(updates_differenceSlice newMessages:{Serializers.VectorToString(newMessages)} newEncryptedMessages:{Serializers.VectorToString(newEncryptedMessages)} other_updates:{Serializers.VectorToString(otherUpdates)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} intermediate_state:{intermediateState})";
        }
    }


    public class UpdatesTooLongConstructor : Updates
    {
        public override Constructor constructor => Constructor.UpdatesTooLong;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe317af7e);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(updatesTooLong)";
        }
    }


    public class UpdateShortMessageConstructor : Updates
    {
        public int id;
        public int fromId;
        public string message;
        public int pts;
        public int date;
        public int seq;

        public UpdateShortMessageConstructor()
        {

        }

        public UpdateShortMessageConstructor(int id, int fromId, string message, int pts, int date, int seq)
        {
            this.id = id;
            this.fromId = fromId;
            this.message = message;
            this.pts = pts;
            this.date = date;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.UpdateShortMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd3f45784);
            writer.Write(id);
            writer.Write(fromId);
            Serializers.String.Write(writer, message);
            writer.Write(pts);
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            fromId = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            pts = reader.ReadInt32();
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateShortMessage id:{id} from_id:{fromId} message:'{message}' pts:{pts} date:{date} seq:{seq})";
        }
    }


    public class UpdateShortChatMessageConstructor : Updates
    {
        public int id;
        public int fromId;
        public int chatId;
        public string message;
        public int pts;
        public int date;
        public int seq;

        public UpdateShortChatMessageConstructor()
        {

        }

        public UpdateShortChatMessageConstructor(int id, int fromId, int chatId, string message, int pts, int date, int seq)
        {
            this.id = id;
            this.fromId = fromId;
            this.chatId = chatId;
            this.message = message;
            this.pts = pts;
            this.date = date;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.UpdateShortChatMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2b2fbd4e);
            writer.Write(id);
            writer.Write(fromId);
            writer.Write(chatId);
            Serializers.String.Write(writer, message);
            writer.Write(pts);
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            fromId = reader.ReadInt32();
            chatId = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            pts = reader.ReadInt32();
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updateShortChatMessage id:{id} from_id:{fromId} chatId:{chatId} message:'{message}' pts:{pts} date:{date} seq:{seq})";
        }
    }


    public class UpdateShortConstructor : Updates
    {
        public Update update;
        public int date;

        public UpdateShortConstructor()
        {

        }

        public UpdateShortConstructor(Update update, int date)
        {
            this.update = update;
            this.date = date;
        }


        public override Constructor constructor => Constructor.UpdateShort;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x78d4dec1);
            update.Write(writer);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            update = Read<Update>(reader);
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateShort update:{update} date:{date})";
        }
    }


    public class UpdatesCombinedConstructor : Updates
    {
        public List<Update> updates;
        public List<User> users;
        public List<Chat> chats;
        public int date;
        public int seqStart;
        public int seq;

        public UpdatesCombinedConstructor()
        {

        }

        public UpdatesCombinedConstructor(List<Update> updates, List<User> users, List<Chat> chats, int date, int seqStart,
            int seq)
        {
            this.updates = updates;
            this.users = users;
            this.chats = chats;
            this.date = date;
            this.seqStart = seqStart;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.UpdatesCombined;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x725b04c3);
            writer.Write(0x1cb5c415);
            writer.Write(updates.Count);
            foreach (Update updatesElement in updates)
            {
                updatesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(date);
            writer.Write(seqStart);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int updatesLen = reader.ReadInt32();
            updates = new List<Update>(updatesLen);
            for (int updatesIndex = 0; updatesIndex < updatesLen; updatesIndex++)
            {
                var updatesElement = Read<Update>(reader);
                updates.Add(updatesElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            date = reader.ReadInt32();
            seqStart = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updatesCombined updates:{Serializers.VectorToString(updates)} users:{Serializers.VectorToString(users)} chats:{Serializers.VectorToString(chats)} date:{date} seq_start:{seqStart} seq:{seq})";
        }
    }


    public class UpdatesConstructor : Updates
    {
        public List<Update> updates;
        public List<User> users;
        public List<Chat> chats;
        public int date;
        public int seq;

        public UpdatesConstructor()
        {

        }

        public UpdatesConstructor(List<Update> updates, List<User> users, List<Chat> chats, int date, int seq)
        {
            this.updates = updates;
            this.users = users;
            this.chats = chats;
            this.date = date;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.Updates;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x74ae4240);
            writer.Write(0x1cb5c415);
            writer.Write(updates.Count);
            foreach (Update updatesElement in updates)
            {
                updatesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int updatesLen = reader.ReadInt32();
            updates = new List<Update>(updatesLen);
            for (int updatesIndex = 0; updatesIndex < updatesLen; updatesIndex++)
            {
                var updatesElement = Read<Update>(reader);
                updates.Add(updatesElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updates updates:{Serializers.VectorToString(updates)} users:{Serializers.VectorToString(users)} chats:{Serializers.VectorToString(chats)} date:{date} seq:{seq})";
        }
    }


    public class PhotosPhotosConstructor : PhotosPhotos
    {
        public List<Photo> photos;
        public List<User> users;

        public PhotosPhotosConstructor()
        {

        }

        public PhotosPhotosConstructor(List<Photo> photos, List<User> users)
        {
            this.photos = photos;
            this.users = users;
        }


        public override Constructor constructor => Constructor.PhotosPhotos;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8dca6aa5);
            writer.Write(0x1cb5c415);
            writer.Write(photos.Count);
            foreach (Photo photosElement in photos)
            {
                photosElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int photosLen = reader.ReadInt32();
            photos = new List<Photo>(photosLen);
            for (int photosIndex = 0; photosIndex < photosLen; photosIndex++)
            {
                var photosElement = Read<Photo>(reader);
                photos.Add(photosElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(photos_photos photos:{Serializers.VectorToString(photos)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class PhotosPhotosSliceConstructor : PhotosPhotos
    {
        public int count;
        public List<Photo> photos;
        public List<User> users;

        public PhotosPhotosSliceConstructor()
        {

        }

        public PhotosPhotosSliceConstructor(int count, List<Photo> photos, List<User> users)
        {
            this.count = count;
            this.photos = photos;
            this.users = users;
        }


        public override Constructor constructor => Constructor.PhotosPhotosSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x15051f54);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(photos.Count);
            foreach (Photo photosElement in photos)
            {
                photosElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int photosLen = reader.ReadInt32();
            photos = new List<Photo>(photosLen);
            for (int photosIndex = 0; photosIndex < photosLen; photosIndex++)
            {
                var photosElement = Read<Photo>(reader);
                photos.Add(photosElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(photos_photosSlice count:{count} photos:{Serializers.VectorToString(photos)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class PhotosPhotoConstructor : PhotosPhoto
    {
        public Photo photo;
        public List<User> users;

        public PhotosPhotoConstructor()
        {

        }

        public PhotosPhotoConstructor(Photo photo, List<User> users)
        {
            this.photo = photo;
            this.users = users;
        }


        public override Constructor constructor => Constructor.PhotosPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x20212ca8);
            photo.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            photo = Read<Photo>(reader);
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return $"(photos_photo photo:{photo} users:{Serializers.VectorToString(users)})";
        }
    }


    public class UploadFileConstructor : UploadFile
    {
        public StorageFileType type;
        public int mtime;
        public byte[] bytes;

        public UploadFileConstructor()
        {

        }

        public UploadFileConstructor(StorageFileType type, int mtime, byte[] bytes)
        {
            this.type = type;
            this.mtime = mtime;
            this.bytes = bytes;
        }

        public override Constructor constructor => Constructor.UploadFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x096a18d5);
            type.Write(writer);
            writer.Write(mtime);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            type = Read<StorageFileType>(reader);
            mtime = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return $"(upload_file type:{type} mtime:{mtime} bytes:{BitConverter.ToString(bytes)})";
        }
    }


    public class DcOptionConstructor : DcOption
    {
        public int id;
        public string hostname;
        public string ipAddress;
        public int port;

        public DcOptionConstructor()
        {

        }

        public DcOptionConstructor(int id, string hostname, string ipAddress, int port)
        {
            this.id = id;
            this.hostname = hostname;
            this.ipAddress = ipAddress;
            this.port = port;
        }


        public override Constructor constructor => Constructor.DcOption;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2ec2a43c);
            writer.Write(id);
            Serializers.String.Write(writer, hostname);
            Serializers.String.Write(writer, ipAddress);
            writer.Write(port);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            hostname = Serializers.String.Read(reader);
            ipAddress = Serializers.String.Read(reader);
            port = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(dcOption id:{id} hostname:'{hostname}' ipAddress:'{ipAddress}' port:{port})";
        }
    }


    public class ConfigConstructor : Config
    {
        public int date;
        public int expires;
        public bool testMode;
        public int thisDc;
        public List<DcOption> dcOptions;
        public int chatBigSize;
        public int chatSizeMax;
        public int broadcastSizeMax;
        public List<DisabledFeature> disabledFeatures;

        public ConfigConstructor()
        {

        }

        public ConfigConstructor(int date, bool testMode, int thisDc, List<DcOption> dcOptions, int chatSizeMax)
        {
            this.date = date;
            this.testMode = testMode;
            this.thisDc = thisDc;
            this.dcOptions = dcOptions;
            this.chatSizeMax = chatSizeMax;
        }

        public override Constructor constructor => Constructor.Config;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7dae33e0);
            writer.Write(date);
            writer.Write(expires);
            WriteBool(writer, testMode);
            writer.Write(thisDc);
            WriteVector(writer, dcOptions);
            writer.Write(chatBigSize);
            writer.Write(chatSizeMax);
            writer.Write(broadcastSizeMax);
            WriteVector(writer, disabledFeatures);
        }

        public override void Read(BinaryReader reader)
        {
            date = reader.ReadInt32();
            expires = reader.ReadInt32();
            testMode = ReadBool(reader);
            thisDc = reader.ReadInt32();
            dcOptions = ReadVector<DcOption>(reader);
            chatBigSize = reader.ReadInt32();
            chatSizeMax = reader.ReadInt32();
            broadcastSizeMax = reader.ReadInt32();
            disabledFeatures = ReadVector<DisabledFeature>(reader);
        }

        public override string ToString()
        {
            return
                $"(config date:{date} testMode:{testMode} thisDc:{thisDc} dcOptions:{Serializers.VectorToString(dcOptions)} chatSizeMax:{chatSizeMax})";
        }
    }


    public class NearestDcConstructor : NearestDc
    {
        public string country;
        public int thisDc;
        public int nearestDc;

        public NearestDcConstructor()
        {

        }

        public NearestDcConstructor(string country, int thisDc, int nearestDc)
        {
            this.country = country;
            this.thisDc = thisDc;
            this.nearestDc = nearestDc;
        }

        public override Constructor constructor => Constructor.NearestDc;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8e1a1775);
            Serializers.String.Write(writer, country);
            writer.Write(thisDc);
            writer.Write(nearestDc);
        }

        public override void Read(BinaryReader reader)
        {
            country = Serializers.String.Read(reader);
            thisDc = reader.ReadInt32();
            nearestDc = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(nearestDc country:'{country}' thisDc:{thisDc} nearestDc:{nearestDc})";
        }
    }


    public class HelpAppUpdateConstructor : HelpAppUpdate
    {
        public int id;
        public bool critical;
        public string url;
        public string text;

        public HelpAppUpdateConstructor()
        {

        }

        public HelpAppUpdateConstructor(int id, bool critical, string url, string text)
        {
            this.id = id;
            this.critical = critical;
            this.url = url;
            this.text = text;
        }

        public override Constructor constructor => Constructor.HelpAppUpdate;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8987f311);
            writer.Write(id);
            writer.Write(critical ? 0x997275b5 : 0xbc799737);
            Serializers.String.Write(writer, url);
            Serializers.String.Write(writer, text);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            critical = reader.ReadUInt32() == 0x997275b5;
            url = Serializers.String.Read(reader);
            text = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(help_appUpdate id:{id} critical:{critical} url:'{url}' text:'{text}')";
        }
    }


    public class HelpNoAppUpdateConstructor : HelpAppUpdate
    {
        public override Constructor constructor => Constructor.HelpNoAppUpdate;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc45a6536);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(help_noAppUpdate)";
        }
    }


    public class HelpInviteTextConstructor : HelpInviteText
    {
        public string message;

        public HelpInviteTextConstructor()
        {

        }

        public HelpInviteTextConstructor(string message)
        {
            this.message = message;
        }


        public override Constructor constructor => Constructor.HelpInviteText;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x18cb9f78);
            Serializers.String.Write(writer, message);
        }

        public override void Read(BinaryReader reader)
        {
            message = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(help_inviteText message:'{message}')";
        }
    }


    public class MessagesStatedMessagesLinksConstructor : MessagesStatedMessages
    {
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;
        public List<ContactsLink> links;
        public int pts;
        public int seq;

        public MessagesStatedMessagesLinksConstructor()
        {

        }

        public MessagesStatedMessagesLinksConstructor(List<Message> messages, List<Chat> chats, List<User> users,
            List<ContactsLink> links, int pts, int seq)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
            this.links = links;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.MessagesStatedMessagesLinks;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3e74f5c6);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(links.Count);
            foreach (ContactsLink linksElement in links)
            {
                linksElement.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<Message>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<Message>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            reader.ReadInt32(); // vector code
            int linksLen = reader.ReadInt32();
            links = new List<ContactsLink>(linksLen);
            for (int linksIndex = 0; linksIndex < linksLen; linksIndex++)
            {
                var linksElement = Read<ContactsLink>(reader);
                links.Add(linksElement);
            }
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(messages_statedMessagesLinks messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} links:{Serializers.VectorToString(links)} pts:{pts} seq:{seq})";
        }
    }


    public class MessagesStatedMessageLinkConstructor : MessagesStatedMessage
    {
        public Message message;
        public List<Chat> chats;
        public List<User> users;
        public List<ContactsLink> links;
        public int pts;
        public int seq;

        public MessagesStatedMessageLinkConstructor()
        {

        }

        public MessagesStatedMessageLinkConstructor(Message message, List<Chat> chats, List<User> users,
            List<ContactsLink> links, int pts, int seq)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
            this.links = links;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.MessagesStatedMessageLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa9af2881);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(links.Count);
            foreach (ContactsLink linksElement in links)
            {
                linksElement.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<Message>(reader);
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            reader.ReadInt32(); // vector code
            int linksLen = reader.ReadInt32();
            links = new List<ContactsLink>(linksLen);
            for (int linksIndex = 0; linksIndex < linksLen; linksIndex++)
            {
                var linksElement = Read<ContactsLink>(reader);
                links.Add(linksElement);
            }
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(messages_statedMessageLink message:{message} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} links:{Serializers.VectorToString(links)} pts:{pts} seq:{seq})";
        }
    }


    public class SentMessageLinkConstructor : SentMessage
    {
        public int id;
        public int date;
        public int pts;
        public int seq;
        public List<ContactsLink> links;

        public SentMessageLinkConstructor()
        {

        }

        public SentMessageLinkConstructor(int id, int date, int pts, int seq, List<ContactsLink> links)
        {
            this.id = id;
            this.date = date;
            this.pts = pts;
            this.seq = seq;
            this.links = links;
        }


        public override Constructor constructor => Constructor.MessagesSentMessageLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe9db4a3f);
            writer.Write(id);
            writer.Write(date);
            writer.Write(pts);
            writer.Write(seq);
            writer.Write(0x1cb5c415);
            writer.Write(links.Count);
            foreach (ContactsLink linksElement in links)
            {
                linksElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            date = reader.ReadInt32();
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int linksLen = reader.ReadInt32();
            links = new List<ContactsLink>(linksLen);
            for (int linksIndex = 0; linksIndex < linksLen; linksIndex++)
            {
                var linksElement = Read<ContactsLink>(reader);
                links.Add(linksElement);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_sentMessageLink id:{id} date:{date} pts:{pts} seq:{seq} links:{Serializers.VectorToString(links)})";
        }
    }


    public class InputGeoChatConstructor : InputGeoChat
    {
        public int chatId;
        public long accessHash;

        public InputGeoChatConstructor()
        {

        }

        public InputGeoChatConstructor(int chatId, long accessHash)
        {
            this.chatId = chatId;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputGeoChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x74d456fa);
            writer.Write(chatId);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputGeoChat chatId:{chatId} accessHash:{accessHash})";
        }
    }


    public class InputNotifyGeoChatPeerConstructor : InputNotifyPeer
    {
        public InputGeoChat peer;

        public InputNotifyGeoChatPeerConstructor()
        {

        }

        public InputNotifyGeoChatPeerConstructor(InputGeoChat peer)
        {
            this.peer = peer;
        }


        public override Constructor constructor => Constructor.InputNotifyGeoChatPeer;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4d8ddec8);
            peer.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            peer = Read<InputGeoChat>(reader);
        }

        public override string ToString()
        {
            return $"(inputNotifyGeoChatPeer peer:{peer})";
        }
    }


    public class GeoChatConstructor : Chat
    {
        public int id;
        public long accessHash;
        public string title;
        public string address;
        public string venue;
        public GeoPoint geo;
        public ChatPhoto photo;
        public int participantsCount;
        public int date;
        public bool checkedIn;
        public int version;

        public GeoChatConstructor()
        {

        }

        public GeoChatConstructor(int id, long accessHash, string title, string address, string venue, GeoPoint geo,
            ChatPhoto photo, int participantsCount, int date, bool checkedIn, int version)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.title = title;
            this.address = address;
            this.venue = venue;
            this.geo = geo;
            this.photo = photo;
            this.participantsCount = participantsCount;
            this.date = date;
            this.checkedIn = checkedIn;
            this.version = version;
        }


        public override Constructor constructor => Constructor.GeoChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x75eaea5a);
            writer.Write(id);
            writer.Write(accessHash);
            Serializers.String.Write(writer, title);
            Serializers.String.Write(writer, address);
            Serializers.String.Write(writer, venue);
            geo.Write(writer);
            photo.Write(writer);
            writer.Write(participantsCount);
            writer.Write(date);
            writer.Write(checkedIn ? 0x997275b5 : 0xbc799737);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            accessHash = reader.ReadInt64();
            title = Serializers.String.Read(reader);
            address = Serializers.String.Read(reader);
            venue = Serializers.String.Read(reader);
            geo = Read<GeoPoint>(reader);
            photo = Read<ChatPhoto>(reader);
            participantsCount = reader.ReadInt32();
            date = reader.ReadInt32();
            checkedIn = reader.ReadUInt32() == 0x997275b5;
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(geoChat id:{id} accessHash:{accessHash} title:'{title}' address:'{address}' venue:'{venue}' geo:{geo} photo:{photo} participants_count:{participantsCount} date:{date} checked_in:{checkedIn} version:{version})";
        }
    }


    public class GeoChatMessageEmptyConstructor : GeoChatMessage
    {
        public int chatId;
        public int id;

        public GeoChatMessageEmptyConstructor()
        {

        }

        public GeoChatMessageEmptyConstructor(int chatId, int id)
        {
            this.chatId = chatId;
            this.id = id;
        }


        public override Constructor constructor => Constructor.GeoChatMessageEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x60311a9b);
            writer.Write(chatId);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(geoChatMessageEmpty chatId:{chatId} id:{id})";
        }
    }


    public class GeoChatMessageConstructor : GeoChatMessage
    {
        public int chatId;
        public int id;
        public int fromId;
        public int date;
        public string message;
        public MessageMedia media;

        public GeoChatMessageConstructor()
        {

        }

        public GeoChatMessageConstructor(int chatId, int id, int fromId, int date, string message, MessageMedia media)
        {
            this.chatId = chatId;
            this.id = id;
            this.fromId = fromId;
            this.date = date;
            this.message = message;
            this.media = media;
        }


        public override Constructor constructor => Constructor.GeoChatMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4505f8e1);
            writer.Write(chatId);
            writer.Write(id);
            writer.Write(fromId);
            writer.Write(date);
            Serializers.String.Write(writer, message);
            media.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            id = reader.ReadInt32();
            fromId = reader.ReadInt32();
            date = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            media = Read<MessageMedia>(reader);
        }

        public override string ToString()
        {
            return
                $"(geoChatMessage chatId:{chatId} id:{id} from_id:{fromId} date:{date} message:'{message}' media:{media})";
        }
    }


    public class GeoChatMessageServiceConstructor : GeoChatMessage
    {
        public int chatId;
        public int id;
        public int fromId;
        public int date;
        public MessageAction action;

        public GeoChatMessageServiceConstructor()
        {

        }

        public GeoChatMessageServiceConstructor(int chatId, int id, int fromId, int date, MessageAction action)
        {
            this.chatId = chatId;
            this.id = id;
            this.fromId = fromId;
            this.date = date;
            this.action = action;
        }


        public override Constructor constructor => Constructor.GeoChatMessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd34fa24e);
            writer.Write(chatId);
            writer.Write(id);
            writer.Write(fromId);
            writer.Write(date);
            action.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            id = reader.ReadInt32();
            fromId = reader.ReadInt32();
            date = reader.ReadInt32();
            action = Read<MessageAction>(reader);
        }

        public override string ToString()
        {
            return $"(geoChatMessageService chatId:{chatId} id:{id} from_id:{fromId} date:{date} action:{action})";
        }
    }


    public class GeochatsStatedMessageConstructor : GeochatsStatedMessage
    {
        public GeoChatMessage message;
        public List<Chat> chats;
        public List<User> users;
        public int seq;

        public GeochatsStatedMessageConstructor()
        {

        }

        public GeochatsStatedMessageConstructor(GeoChatMessage message, List<Chat> chats, List<User> users, int seq)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
            this.seq = seq;
        }


        public override Constructor constructor => Constructor.GeochatsStatedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x17b1578b);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<GeoChatMessage>(reader);
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(geochats_statedMessage message:{message} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} seq:{seq})";
        }
    }


    public class GeochatsLocatedConstructor : GeochatsLocated
    {
        public List<ChatLocated> results;
        public List<GeoChatMessage> messages;
        public List<Chat> chats;
        public List<User> users;

        public GeochatsLocatedConstructor()
        {

        }

        public GeochatsLocatedConstructor(List<ChatLocated> results, List<GeoChatMessage> messages, List<Chat> chats,
            List<User> users)
        {
            this.results = results;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.GeochatsLocated;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x48feb267);
            writer.Write(0x1cb5c415);
            writer.Write(results.Count);
            foreach (ChatLocated resultsElement in results)
            {
                resultsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (GeoChatMessage messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int resultsLen = reader.ReadInt32();
            results = new List<ChatLocated>(resultsLen);
            for (int resultsIndex = 0; resultsIndex < resultsLen; resultsIndex++)
            {
                var resultsElement = Read<ChatLocated>(reader);
                results.Add(resultsElement);
            }
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<GeoChatMessage>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<GeoChatMessage>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(geochats_located results:{Serializers.VectorToString(results)} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class GeochatsMessagesConstructor : GeochatsMessages
    {
        public List<GeoChatMessage> messages;
        public List<Chat> chats;
        public List<User> users;

        public GeochatsMessagesConstructor()
        {

        }

        public GeochatsMessagesConstructor(List<GeoChatMessage> messages, List<Chat> chats, List<User> users)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.GeochatsMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd1526db1);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (GeoChatMessage messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<GeoChatMessage>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<GeoChatMessage>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(geochats_messages messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class GeochatsMessagesSliceConstructor : GeochatsMessages
    {
        public int count;
        public List<GeoChatMessage> messages;
        public List<Chat> chats;
        public List<User> users;

        public GeochatsMessagesSliceConstructor()
        {

        }

        public GeochatsMessagesSliceConstructor(int count, List<GeoChatMessage> messages, List<Chat> chats, List<User> users)
        {
            this.count = count;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor constructor => Constructor.GeochatsMessagesSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xbc5863e8);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (GeoChatMessage messagesElement in messages)
            {
                messagesElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chatsElement in chats)
            {
                chatsElement.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User usersElement in users)
            {
                usersElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int messagesLen = reader.ReadInt32();
            messages = new List<GeoChatMessage>(messagesLen);
            for (int messagesIndex = 0; messagesIndex < messagesLen; messagesIndex++)
            {
                var messagesElement = Read<GeoChatMessage>(reader);
                messages.Add(messagesElement);
            }
            reader.ReadInt32(); // vector code
            int chatsLen = reader.ReadInt32();
            chats = new List<Chat>(chatsLen);
            for (int chatsIndex = 0; chatsIndex < chatsLen; chatsIndex++)
            {
                var chatsElement = Read<Chat>(reader);
                chats.Add(chatsElement);
            }
            reader.ReadInt32(); // vector code
            int usersLen = reader.ReadInt32();
            users = new List<User>(usersLen);
            for (int usersIndex = 0; usersIndex < usersLen; usersIndex++)
            {
                var usersElement = Read<User>(reader);
                users.Add(usersElement);
            }
        }

        public override string ToString()
        {
            return
                $"(geochats_messagesSlice count:{count} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class MessageActionGeoChatCreateConstructor : MessageAction
    {
        public string title;
        public string address;

        public MessageActionGeoChatCreateConstructor()
        {

        }

        public MessageActionGeoChatCreateConstructor(string title, string address)
        {
            this.title = title;
            this.address = address;
        }


        public override Constructor constructor => Constructor.MessageActionGeoChatCreate;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6f038ebc);
            Serializers.String.Write(writer, title);
            Serializers.String.Write(writer, address);
        }

        public override void Read(BinaryReader reader)
        {
            title = Serializers.String.Read(reader);
            address = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(messageActionGeoChatCreate title:'{title}' address:'{address}')";
        }
    }


    public class MessageActionGeoChatCheckinConstructor : MessageAction
    {
        public override Constructor constructor => Constructor.MessageActionGeoChatCheckin;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0c7d53de);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(messageActionGeoChatCheckin)";
        }
    }


    public class UpdateNewGeoChatMessageConstructor : Update
    {
        public GeoChatMessage message;

        public UpdateNewGeoChatMessageConstructor()
        {

        }

        public UpdateNewGeoChatMessageConstructor(GeoChatMessage message)
        {
            this.message = message;
        }


        public override Constructor constructor => Constructor.UpdateNewGeoChatMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5a68e3f7);
            message.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<GeoChatMessage>(reader);
        }

        public override string ToString()
        {
            return $"(updateNewGeoChatMessage message:{message})";
        }
    }


    public class WallPaperSolidConstructor : WallPaper
    {
        public int id;
        public string title;
        public int bgColor;
        public int color;

        public WallPaperSolidConstructor()
        {

        }

        public WallPaperSolidConstructor(int id, string title, int bgColor, int color)
        {
            this.id = id;
            this.title = title;
            this.bgColor = bgColor;
            this.color = color;
        }


        public override Constructor constructor => Constructor.WallPaperSolid;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x63117f24);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            writer.Write(bgColor);
            writer.Write(color);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            bgColor = reader.ReadInt32();
            color = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(wallPaperSolid id:{id} title:'{title}' bgColor:{bgColor} color:{color})";
        }
    }


    public class UpdateNewEncryptedMessageConstructor : Update
    {
        public EncryptedMessage message;
        public int qts;

        public UpdateNewEncryptedMessageConstructor()
        {

        }

        public UpdateNewEncryptedMessageConstructor(EncryptedMessage message, int qts)
        {
            this.message = message;
            this.qts = qts;
        }


        public override Constructor constructor => Constructor.UpdateNewEncryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x12bcbd9a);
            message.Write(writer);
            writer.Write(qts);
        }

        public override void Read(BinaryReader reader)
        {
            message = Read<EncryptedMessage>(reader);
            qts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateNewEncryptedMessage message:{message} qts:{qts})";
        }
    }


    public class UpdateEncryptedChatTypingConstructor : Update
    {
        public int chatId;

        public UpdateEncryptedChatTypingConstructor()
        {

        }

        public UpdateEncryptedChatTypingConstructor(int chatId)
        {
            this.chatId = chatId;
        }


        public override Constructor constructor => Constructor.UpdateEncryptedChatTyping;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1710f156);
            writer.Write(chatId);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateEncryptedChatTyping chatId:{chatId})";
        }
    }


    public class UpdateEncryptionConstructor : Update
    {
        public EncryptedChat chat;
        public int date;

        public UpdateEncryptionConstructor()
        {

        }

        public UpdateEncryptionConstructor(EncryptedChat chat, int date)
        {
            this.chat = chat;
            this.date = date;
        }


        public override Constructor constructor => Constructor.UpdateEncryption;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb4a2e88d);
            chat.Write(writer);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            chat = Read<EncryptedChat>(reader);
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateEncryption chat:{chat} date:{date})";
        }
    }


    public class UpdateEncryptedMessagesReadConstructor : Update
    {
        public int chatId;
        public int maxDate;
        public int date;

        public UpdateEncryptedMessagesReadConstructor()
        {

        }

        public UpdateEncryptedMessagesReadConstructor(int chatId, int maxDate, int date)
        {
            this.chatId = chatId;
            this.maxDate = maxDate;
            this.date = date;
        }


        public override Constructor constructor => Constructor.UpdateEncryptedMessagesRead;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x38fe25b7);
            writer.Write(chatId);
            writer.Write(maxDate);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            maxDate = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateEncryptedMessagesRead chatId:{chatId} max_date:{maxDate} date:{date})";
        }
    }


    public class EncryptedChatEmptyConstructor : EncryptedChat
    {
        public int id;

        public EncryptedChatEmptyConstructor()
        {

        }

        public EncryptedChatEmptyConstructor(int id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.EncryptedChatEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xab7ec0a0);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(encryptedChatEmpty id:{id})";
        }
    }


    public class EncryptedChatWaitingConstructor : EncryptedChat
    {
        public int id;
        public long accessHash;
        public int date;
        public int adminId;
        public int participantId;

        public EncryptedChatWaitingConstructor()
        {

        }

        public EncryptedChatWaitingConstructor(int id, long accessHash, int date, int adminId, int participantId)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.date = date;
            this.adminId = adminId;
            this.participantId = participantId;
        }


        public override Constructor constructor => Constructor.EncryptedChatWaiting;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3bf703dc);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(date);
            writer.Write(adminId);
            writer.Write(participantId);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            accessHash = reader.ReadInt64();
            date = reader.ReadInt32();
            adminId = reader.ReadInt32();
            participantId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(encryptedChatWaiting id:{id} accessHash:{accessHash} date:{date} admin_id:{adminId} participant_id:{participantId})";
        }
    }


    public class EncryptedChatRequestedConstructor : EncryptedChat
    {
        public int id;
        public long accessHash;
        public int date;
        public int adminId;
        public int participantId;
        public byte[] gA;
        public byte[] nonce;

        public EncryptedChatRequestedConstructor()
        {

        }

        public EncryptedChatRequestedConstructor(int id, long accessHash, int date, int adminId, int participantId,
            byte[] gA, byte[] nonce)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.date = date;
            this.adminId = adminId;
            this.participantId = participantId;
            this.gA = gA;
            this.nonce = nonce;
        }


        public override Constructor constructor => Constructor.EncryptedChatRequested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfda9a7b7);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(date);
            writer.Write(adminId);
            writer.Write(participantId);
            Serializers.Bytes.Write(writer, gA);
            Serializers.Bytes.Write(writer, nonce);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            accessHash = reader.ReadInt64();
            date = reader.ReadInt32();
            adminId = reader.ReadInt32();
            participantId = reader.ReadInt32();
            gA = Serializers.Bytes.Read(reader);
            nonce = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(encryptedChatRequested id:{id} accessHash:{accessHash} date:{date} admin_id:{adminId} participant_id:{participantId} g_a:{BitConverter.ToString(gA)} nonce:{BitConverter.ToString(nonce)})";
        }
    }


    public class EncryptedChatConstructor : EncryptedChat
    {
        public int id;
        public long accessHash;
        public int date;
        public int adminId;
        public int participantId;
        public byte[] gAorB;
        public byte[] nonce;
        public long keyFingerprint;

        public EncryptedChatConstructor()
        {

        }

        public EncryptedChatConstructor(int id, long accessHash, int date, int adminId, int participantId, byte[] gAorB,
            byte[] nonce, long keyFingerprint)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.date = date;
            this.adminId = adminId;
            this.participantId = participantId;
            this.gAorB = gAorB;
            this.nonce = nonce;
            this.keyFingerprint = keyFingerprint;
        }


        public override Constructor constructor => Constructor.EncryptedChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6601d14f);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(date);
            writer.Write(adminId);
            writer.Write(participantId);
            Serializers.Bytes.Write(writer, gAorB);
            Serializers.Bytes.Write(writer, nonce);
            writer.Write(keyFingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            accessHash = reader.ReadInt64();
            date = reader.ReadInt32();
            adminId = reader.ReadInt32();
            participantId = reader.ReadInt32();
            gAorB = Serializers.Bytes.Read(reader);
            nonce = Serializers.Bytes.Read(reader);
            keyFingerprint = reader.ReadInt64();
        }

        public override string ToString()
        {
            return
                $"(encryptedChat id:{id} accessHash:{accessHash} date:{date} admin_id:{adminId} participant_id:{participantId} g_a_or_b:{BitConverter.ToString(gAorB)} nonce:{BitConverter.ToString(nonce)} key_fingerprint:{keyFingerprint})";
        }
    }


    public class EncryptedChatDiscardedConstructor : EncryptedChat
    {
        public int id;

        public EncryptedChatDiscardedConstructor()
        {

        }

        public EncryptedChatDiscardedConstructor(int id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.EncryptedChatDiscarded;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x13d6dd27);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(encryptedChatDiscarded id:{id})";
        }
    }


    public class InputEncryptedChatConstructor : InputEncryptedChat
    {
        public int chatId;
        public long accessHash;

        public InputEncryptedChatConstructor()
        {

        }

        public InputEncryptedChatConstructor(int chatId, long accessHash)
        {
            this.chatId = chatId;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputEncryptedChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf141b5e1);
            writer.Write(chatId);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputEncryptedChat chatId:{chatId} accessHash:{accessHash})";
        }
    }


    public class EncryptedFileEmptyConstructor : EncryptedFile
    {
        public override Constructor constructor => Constructor.EncryptedFileEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc21f497e);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(encryptedFileEmpty)";
        }
    }


    public class EncryptedFileConstructor : EncryptedFile
    {
        public long id;
        public long accessHash;
        public int size;
        public int dcId;
        public int keyFingerprint;

        public EncryptedFileConstructor()
        {

        }

        public EncryptedFileConstructor(long id, long accessHash, int size, int dcId, int keyFingerprint)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.size = size;
            this.dcId = dcId;
            this.keyFingerprint = keyFingerprint;
        }


        public override Constructor constructor => Constructor.EncryptedFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4a70994c);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(size);
            writer.Write(dcId);
            writer.Write(keyFingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
            size = reader.ReadInt32();
            dcId = reader.ReadInt32();
            keyFingerprint = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(encryptedFile id:{id} accessHash:{accessHash} size:{size} dc_id:{dcId} key_fingerprint:{keyFingerprint})";
        }
    }


    public class InputEncryptedFileEmptyConstructor : InputEncryptedFile
    {
        public override Constructor constructor => Constructor.InputEncryptedFileEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1837c364);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputEncryptedFileEmpty)";
        }
    }


    public class InputEncryptedFileUploadedConstructor : InputEncryptedFile
    {
        public long id;
        public int parts;
        public string md5Checksum;
        public int keyFingerprint;

        public InputEncryptedFileUploadedConstructor()
        {

        }

        public InputEncryptedFileUploadedConstructor(long id, int parts, string md5Checksum, int keyFingerprint)
        {
            this.id = id;
            this.parts = parts;
            this.md5Checksum = md5Checksum;
            this.keyFingerprint = keyFingerprint;
        }


        public override Constructor constructor => Constructor.InputEncryptedFileUploaded;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x64bd0306);
            writer.Write(id);
            writer.Write(parts);
            Serializers.String.Write(writer, md5Checksum);
            writer.Write(keyFingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            md5Checksum = Serializers.String.Read(reader);
            keyFingerprint = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(inputEncryptedFileUploaded id:{id} parts:{parts} md5Checksum:'{md5Checksum}' keyFingerprint:{keyFingerprint})";
        }
    }


    public class InputEncryptedFileConstructor : InputEncryptedFile
    {
        public long id;
        public long accessHash;

        public InputEncryptedFileConstructor()
        {

        }

        public InputEncryptedFileConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputEncryptedFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5a17b5e5);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputEncryptedFile id:{id} accessHash:{accessHash})";
        }
    }


    public class InputEncryptedFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long accessHash;

        public InputEncryptedFileLocationConstructor()
        {

        }

        public InputEncryptedFileLocationConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputEncryptedFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf5235d55);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputEncryptedFileLocation id:{id} accessHash:{accessHash})";
        }
    }


    public class EncryptedMessageConstructor : EncryptedMessage
    {
        public long randomId;
        public int chatId;
        public int date;
        public byte[] bytes;
        public EncryptedFile file;

        public EncryptedMessageConstructor()
        {

        }

        public EncryptedMessageConstructor(long randomId, int chatId, int date, byte[] bytes, EncryptedFile file)
        {
            this.randomId = randomId;
            this.chatId = chatId;
            this.date = date;
            this.bytes = bytes;
            this.file = file;
        }


        public override Constructor constructor => Constructor.EncryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xed18c118);
            writer.Write(randomId);
            writer.Write(chatId);
            writer.Write(date);
            Serializers.Bytes.Write(writer, bytes);
            file.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            randomId = reader.ReadInt64();
            chatId = reader.ReadInt32();
            date = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
            file = Read<EncryptedFile>(reader);
        }

        public override string ToString()
        {
            return
                $"(encryptedMessage randomId:{randomId} chatId:{chatId} date:{date} bytes:{BitConverter.ToString(bytes)} file:{file})";
        }
    }


    public class EncryptedMessageServiceConstructor : EncryptedMessage
    {
        public long randomId;
        public int chatId;
        public int date;
        public byte[] bytes;

        public EncryptedMessageServiceConstructor()
        {

        }

        public EncryptedMessageServiceConstructor(long randomId, int chatId, int date, byte[] bytes)
        {
            this.randomId = randomId;
            this.chatId = chatId;
            this.date = date;
            this.bytes = bytes;
        }


        public override Constructor constructor => Constructor.EncryptedMessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x23734b06);
            writer.Write(randomId);
            writer.Write(chatId);
            writer.Write(date);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            randomId = reader.ReadInt64();
            chatId = reader.ReadInt32();
            date = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(encryptedMessageService randomId:{randomId} chatId:{chatId} date:{date} bytes:{BitConverter.ToString(bytes)})";
        }
    }


    public class DecryptedMessageLayerConstructor : DecryptedMessageLayer
    {
        public int layer;
        public DecryptedMessage message;

        public DecryptedMessageLayerConstructor()
        {

        }

        public DecryptedMessageLayerConstructor(int layer, DecryptedMessage message)
        {
            this.layer = layer;
            this.message = message;
        }


        public override Constructor constructor => Constructor.DecryptedMessageLayer;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x99a438cf);
            writer.Write(layer);
            message.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            layer = reader.ReadInt32();
            message = Read<DecryptedMessage>(reader);
        }

        public override string ToString()
        {
            return $"(decryptedMessageLayer layer:{layer} message:{message})";
        }
    }


    public class DecryptedMessageConstructor : DecryptedMessage
    {
        public long randomId;
        public byte[] randomBytes;
        public string message;
        public DecryptedMessageMedia media;

        public DecryptedMessageConstructor()
        {

        }

        public DecryptedMessageConstructor(long randomId, byte[] randomBytes, string message, DecryptedMessageMedia media)
        {
            this.randomId = randomId;
            this.randomBytes = randomBytes;
            this.message = message;
            this.media = media;
        }


        public override Constructor constructor => Constructor.DecryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1f814f1f);
            writer.Write(randomId);
            Serializers.Bytes.Write(writer, randomBytes);
            Serializers.String.Write(writer, message);
            media.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            randomId = reader.ReadInt64();
            randomBytes = Serializers.Bytes.Read(reader);
            message = Serializers.String.Read(reader);
            media = Read<DecryptedMessageMedia>(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessage randomId:{randomId} random_bytes:{BitConverter.ToString(randomBytes)} message:'{message}' media:{media})";
        }
    }


    public class DecryptedMessageServiceConstructor : DecryptedMessage
    {
        public long randomId;
        public byte[] randomBytes;
        public DecryptedMessageAction action;

        public DecryptedMessageServiceConstructor()
        {

        }

        public DecryptedMessageServiceConstructor(long randomId, byte[] randomBytes, DecryptedMessageAction action)
        {
            this.randomId = randomId;
            this.randomBytes = randomBytes;
            this.action = action;
        }


        public override Constructor constructor => Constructor.DecryptedMessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xaa48327d);
            writer.Write(randomId);
            Serializers.Bytes.Write(writer, randomBytes);
            action.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            randomId = reader.ReadInt64();
            randomBytes = Serializers.Bytes.Read(reader);
            action = Read<DecryptedMessageAction>(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageService randomId:{randomId} random_bytes:{BitConverter.ToString(randomBytes)} action:{action})";
        }
    }


    public class DecryptedMessageMediaEmptyConstructor : DecryptedMessageMedia
    {
        public override Constructor constructor => Constructor.DecryptedMessageMediaEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x089f5c4a);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(decryptedMessageMediaEmpty)";
        }
    }


    public class DecryptedMessageMediaPhotoConstructor : DecryptedMessageMedia
    {
        public byte[] thumb;
        public int thumbW;
        public int thumbH;
        public int w;
        public int h;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaPhotoConstructor()
        {

        }

        public DecryptedMessageMediaPhotoConstructor(byte[] thumb, int thumbW, int thumbH, int w, int h, int size,
            byte[] key, byte[] iv)
        {
            this.thumb = thumb;
            this.thumbW = thumbW;
            this.thumbH = thumbH;
            this.w = w;
            this.h = h;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor constructor => Constructor.DecryptedMessageMediaPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x32798a8c);
            Serializers.Bytes.Write(writer, thumb);
            writer.Write(thumbW);
            writer.Write(thumbH);
            writer.Write(w);
            writer.Write(h);
            writer.Write(size);
            Serializers.Bytes.Write(writer, key);
            Serializers.Bytes.Write(writer, iv);
        }

        public override void Read(BinaryReader reader)
        {
            thumb = Serializers.Bytes.Read(reader);
            thumbW = reader.ReadInt32();
            thumbH = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
            size = reader.ReadInt32();
            key = Serializers.Bytes.Read(reader);
            iv = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaPhoto thumb:{BitConverter.ToString(thumb)} thumbW:{thumbW} thumbH:{thumbH} w:{w} h:{h} size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
        }
    }


    public class DecryptedMessageMediaVideoConstructor : DecryptedMessageMedia
    {
        public byte[] thumb;
        public int thumbW;
        public int thumbH;
        public int duration;
        public int w;
        public int h;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaVideoConstructor()
        {

        }

        public DecryptedMessageMediaVideoConstructor(byte[] thumb, int thumbW, int thumbH, int duration, int w, int h,
            int size, byte[] key, byte[] iv)
        {
            this.thumb = thumb;
            this.thumbW = thumbW;
            this.thumbH = thumbH;
            this.duration = duration;
            this.w = w;
            this.h = h;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor constructor => Constructor.DecryptedMessageMediaVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4cee6ef3);
            Serializers.Bytes.Write(writer, thumb);
            writer.Write(thumbW);
            writer.Write(thumbH);
            writer.Write(duration);
            writer.Write(w);
            writer.Write(h);
            writer.Write(size);
            Serializers.Bytes.Write(writer, key);
            Serializers.Bytes.Write(writer, iv);
        }

        public override void Read(BinaryReader reader)
        {
            thumb = Serializers.Bytes.Read(reader);
            thumbW = reader.ReadInt32();
            thumbH = reader.ReadInt32();
            duration = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
            size = reader.ReadInt32();
            key = Serializers.Bytes.Read(reader);
            iv = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaVideo thumb:{BitConverter.ToString(thumb)} thumbW:{thumbW} thumbH:{thumbH} duration:{duration} w:{w} h:{h} size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
        }
    }


    public class DecryptedMessageMediaGeoPointConstructor : DecryptedMessageMedia
    {
        public double lat;
        public double lng;

        public DecryptedMessageMediaGeoPointConstructor()
        {

        }

        public DecryptedMessageMediaGeoPointConstructor(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }


        public override Constructor constructor => Constructor.DecryptedMessageMediaGeoPoint;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x35480a59);
            writer.Write(lat);
            writer.Write(lng);
        }

        public override void Read(BinaryReader reader)
        {
            lat = reader.ReadDouble();
            lng = reader.ReadDouble();
        }

        public override string ToString()
        {
            return $"(decryptedMessageMediaGeoPoint lat:{lat} long:{lng})";
        }
    }


    public class DecryptedMessageMediaContactConstructor : DecryptedMessageMedia
    {
        public string phoneNumber;
        public string firstName;
        public string lastName;
        public int userId;

        public DecryptedMessageMediaContactConstructor()
        {

        }

        public DecryptedMessageMediaContactConstructor(string phoneNumber, string firstName, string lastName, int userId)
        {
            this.phoneNumber = phoneNumber;
            this.firstName = firstName;
            this.lastName = lastName;
            this.userId = userId;
        }


        public override Constructor constructor => Constructor.DecryptedMessageMediaContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x588a0a97);
            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            writer.Write(userId);
        }

        public override void Read(BinaryReader reader)
        {
            phoneNumber = Serializers.String.Read(reader);
            firstName = Serializers.String.Read(reader);
            lastName = Serializers.String.Read(reader);
            userId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaContact phoneNumber:'{phoneNumber}' firstName:'{firstName}' lastName:'{lastName}' userId:{userId})";
        }
    }


    public class DecryptedMessageActionSetMessageTtlConstructor : DecryptedMessageAction
    {
        public int ttlSeconds;

        public DecryptedMessageActionSetMessageTtlConstructor()
        {

        }

        public DecryptedMessageActionSetMessageTtlConstructor(int ttlSeconds)
        {
            this.ttlSeconds = ttlSeconds;
        }


        public override Constructor constructor => Constructor.DecryptedMessageActionSetMessageTtl;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa1733aec);
            writer.Write(ttlSeconds);
        }

        public override void Read(BinaryReader reader)
        {
            ttlSeconds = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(decryptedMessageActionSetMessageTTL ttlSeconds:{ttlSeconds})";
        }
    }


    public class MessagesDhConfigNotModifiedConstructor : MessagesDhConfig
    {
        public byte[] random;

        public MessagesDhConfigNotModifiedConstructor()
        {

        }

        public MessagesDhConfigNotModifiedConstructor(byte[] random)
        {
            this.random = random;
        }


        public override Constructor constructor => Constructor.MessagesDhConfigNotModified;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc0e24635);
            Serializers.Bytes.Write(writer, random);
        }

        public override void Read(BinaryReader reader)
        {
            random = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return $"(messages_dhConfigNotModified random:{BitConverter.ToString(random)})";
        }
    }


    public class MessagesDhConfigConstructor : MessagesDhConfig
    {
        public int g;
        public byte[] p;
        public int version;
        public byte[] random;

        public MessagesDhConfigConstructor()
        {

        }

        public MessagesDhConfigConstructor(int g, byte[] p, int version, byte[] random)
        {
            this.g = g;
            this.p = p;
            this.version = version;
            this.random = random;
        }


        public override Constructor constructor => Constructor.MessagesDhConfig;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2c221edd);
            writer.Write(g);
            Serializers.Bytes.Write(writer, p);
            writer.Write(version);
            Serializers.Bytes.Write(writer, random);
        }

        public override void Read(BinaryReader reader)
        {
            g = reader.ReadInt32();
            p = Serializers.Bytes.Read(reader);
            version = reader.ReadInt32();
            random = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(messages_dhConfig g:{g} p:{BitConverter.ToString(p)} version:{version} random:{BitConverter.ToString(random)})";
        }
    }


    public class MessagesSentEncryptedMessageConstructor : MessagesSentEncryptedMessage
    {
        public int date;

        public MessagesSentEncryptedMessageConstructor()
        {

        }

        public MessagesSentEncryptedMessageConstructor(int date)
        {
            this.date = date;
        }


        public override Constructor constructor => Constructor.MessagesSentEncryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x560f8935);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messages_sentEncryptedMessage date:{date})";
        }
    }


    public class MessagesSentEncryptedFileConstructor : MessagesSentEncryptedMessage
    {
        public int date;
        public EncryptedFile file;

        public MessagesSentEncryptedFileConstructor()
        {

        }

        public MessagesSentEncryptedFileConstructor(int date, EncryptedFile file)
        {
            this.date = date;
            this.file = file;
        }


        public override Constructor constructor => Constructor.MessagesSentEncryptedFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9493ff32);
            writer.Write(date);
            file.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            date = reader.ReadInt32();
            file = Read<EncryptedFile>(reader);
        }

        public override string ToString()
        {
            return $"(messages_sentEncryptedFile date:{date} file:{file})";
        }
    }


    public class InputFileBigConstructor : InputFile
    {
        public long id;
        public int parts;
        public string name;

        public InputFileBigConstructor()
        {

        }

        public InputFileBigConstructor(long id, int parts, string name)
        {
            this.id = id;
            this.parts = parts;
            this.name = name;
        }


        public override Constructor constructor => Constructor.InputFileBig;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfa4f0bb5);
            writer.Write(id);
            writer.Write(parts);
            Serializers.String.Write(writer, name);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            name = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(inputFileBig id:{id} parts:{parts} name:'{name}')";
        }
    }


    public class InputEncryptedFileBigUploadedConstructor : InputEncryptedFile
    {
        public long id;
        public int parts;
        public int keyFingerprint;

        public InputEncryptedFileBigUploadedConstructor()
        {

        }

        public InputEncryptedFileBigUploadedConstructor(long id, int parts, int keyFingerprint)
        {
            this.id = id;
            this.parts = parts;
            this.keyFingerprint = keyFingerprint;
        }


        public override Constructor constructor => Constructor.InputEncryptedFileBigUploaded;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2dc173c8);
            writer.Write(id);
            writer.Write(parts);
            writer.Write(keyFingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            keyFingerprint = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputEncryptedFileBigUploaded id:{id} parts:{parts} keyFingerprint:{keyFingerprint})";
        }
    }


    public class UpdateChatParticipantAddConstructor : Update
    {
        public int chatId;
        public int userId;
        public int inviterId;
        public int version;

        public UpdateChatParticipantAddConstructor()
        {

        }

        public UpdateChatParticipantAddConstructor(int chatId, int userId, int inviterId, int version)
        {
            this.chatId = chatId;
            this.userId = userId;
            this.inviterId = inviterId;
            this.version = version;
        }


        public override Constructor constructor => Constructor.UpdateChatParticipantAdd;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3a0eeb22);
            writer.Write(chatId);
            writer.Write(userId);
            writer.Write(inviterId);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            userId = reader.ReadInt32();
            inviterId = reader.ReadInt32();
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updateChatParticipantAdd chatId:{chatId} userId:{userId} inviter_id:{inviterId} version:{version})";
        }
    }


    public class UpdateChatParticipantDeleteConstructor : Update
    {
        public int chatId;
        public int userId;
        public int version;

        public UpdateChatParticipantDeleteConstructor()
        {

        }

        public UpdateChatParticipantDeleteConstructor(int chatId, int userId, int version)
        {
            this.chatId = chatId;
            this.userId = userId;
            this.version = version;
        }


        public override Constructor constructor => Constructor.UpdateChatParticipantDelete;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6e5f8c22);
            writer.Write(chatId);
            writer.Write(userId);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            userId = reader.ReadInt32();
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateChatParticipantDelete chatId:{chatId} userId:{userId} version:{version})";
        }
    }


    public class UpdateDcOptionsConstructor : Update
    {
        public List<DcOption> dcOptions;

        public UpdateDcOptionsConstructor()
        {

        }

        public UpdateDcOptionsConstructor(List<DcOption> dcOptions)
        {
            this.dcOptions = dcOptions;
        }


        public override Constructor constructor => Constructor.UpdateDcOptions;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8e5e9873);
            writer.Write(0x1cb5c415);
            writer.Write(dcOptions.Count);
            foreach (DcOption dcOptionsElement in dcOptions)
            {
                dcOptionsElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int dcOptionsLen = reader.ReadInt32();
            dcOptions = new List<DcOption>(dcOptionsLen);
            for (int dcOptionsIndex = 0; dcOptionsIndex < dcOptionsLen; dcOptionsIndex++)
            {
                var dcOptionsElement = Read<DcOption>(reader);
                dcOptions.Add(dcOptionsElement);
            }
        }

        public override string ToString()
        {
            return $"(updateDcOptions dcOptions:{Serializers.VectorToString(dcOptions)})";
        }
    }

    public class UpdateUserBlockedConstructor : Update
    {
        public int userId;
        public bool blocked;

        public override Constructor constructor => Constructor.UpdateUserBlocked;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            blocked = reader.ReadBoolean();
        }

        public override string ToString()
        {
            return $"(updateUserBlocked userId:{userId}, blocked:{blocked})";
        }
    }

    public class UpdateNotifySettingsConstructor : Update
    {
        public InputNotifyPeer peer;
        public PeerNotifySettings notifySettings;

        public override Constructor constructor => Constructor.UpdateNotifySettings;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            peer = Read<InputNotifyPeer>(reader);
            notifySettings = Read<PeerNotifySettings>(reader);
        }

        public override string ToString()
        {
            return $"(updateNotifySettings peer:{peer}, notifySettings:{notifySettings})";
        }
    }

    public class UpdateServiceNotificationConstructor : Update
    {
        public string type;
        public string message;
        public MessageMedia media;
        public bool popup;

        public override Constructor constructor => Constructor.UpdateServiceNotification;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            type = Serializers.String.Read(reader);
            message = Serializers.String.Read(reader);
            media = Read<MessageMedia>(reader);
            popup = reader.ReadBoolean();
        }

        public override string ToString()
        {
            return $"(updateServiceNotification type:{type}, message:{message}, media:{media}, popup:{popup})";
        }
    }

    public class UpdatePrivacyConstructor : Update
    {
        public override Constructor constructor => Constructor.UpdatePrivacy;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            throw new Exception("No description in TLSchema for this type. Its broken :(");
        }
    }

    public class UpdateUserPhoneConstructor : Update
    {
        public int userId;
        public string phone;

        public override Constructor constructor => Constructor.UpdateUserPhone;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            phone = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(updateUserPhone userId:{userId}, phone:{phone})";
        }
    }

    public class InputMediaUploadedAudioConstructor : InputMedia
    {
        public InputFile file;
        public int duration;

        public InputMediaUploadedAudioConstructor()
        {

        }

        public InputMediaUploadedAudioConstructor(InputFile file, int duration)
        {
            this.file = file;
            this.duration = duration;
        }


        public override Constructor constructor => Constructor.InputMediaUploadedAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x61a6d436);
            file.Write(writer);
            writer.Write(duration);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
            duration = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputMediaUploadedAudio file:{file} duration:{duration})";
        }
    }


    public class InputMediaAudioConstructor : InputMedia
    {
        public InputAudio id;

        public InputMediaAudioConstructor()
        {

        }

        public InputMediaAudioConstructor(InputAudio id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.InputMediaAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x89938781);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = Read<InputAudio>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaAudio id:{id})";
        }
    }


    public class InputMediaUploadedDocumentConstructor : InputMedia
    {
        public InputFile file;
        public string fileName;
        public string mimeType;

        public InputMediaUploadedDocumentConstructor()
        {

        }

        public InputMediaUploadedDocumentConstructor(InputFile file, string fileName, string mimeType)
        {
            this.file = file;
            this.fileName = fileName;
            this.mimeType = mimeType;
        }


        public override Constructor constructor => Constructor.InputMediaUploadedDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x34e794bd);
            file.Write(writer);
            Serializers.String.Write(writer, fileName);
            Serializers.String.Write(writer, mimeType);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
            fileName = Serializers.String.Read(reader);
            mimeType = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaUploadedDocument file:{file} fileName:'{fileName}' mime_type:'{mimeType}')";
        }
    }


    public class InputMediaUploadedThumbDocumentConstructor : InputMedia
    {
        public InputFile file;
        public InputFile thumb;
        public string fileName;
        public string mimeType;

        public InputMediaUploadedThumbDocumentConstructor()
        {

        }

        public InputMediaUploadedThumbDocumentConstructor(InputFile file, InputFile thumb, string fileName, string mimeType)
        {
            this.file = file;
            this.thumb = thumb;
            this.fileName = fileName;
            this.mimeType = mimeType;
        }


        public override Constructor constructor => Constructor.InputMediaUploadedThumbDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3e46de5d);
            file.Write(writer);
            thumb.Write(writer);
            Serializers.String.Write(writer, fileName);
            Serializers.String.Write(writer, mimeType);
        }

        public override void Read(BinaryReader reader)
        {
            file = Read<InputFile>(reader);
            thumb = Read<InputFile>(reader);
            fileName = Serializers.String.Read(reader);
            mimeType = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(inputMediaUploadedThumbDocument file:{file} thumb:{thumb} fileName:'{fileName}' mime_type:'{mimeType}')";
        }
    }


    public class InputMediaDocumentConstructor : InputMedia
    {
        public InputDocument id;

        public InputMediaDocumentConstructor()
        {

        }

        public InputMediaDocumentConstructor(InputDocument id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.InputMediaDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd184e841);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = Read<InputDocument>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaDocument id:{id})";
        }
    }


    public class MessageMediaDocumentConstructor : MessageMedia
    {
        public Document document;

        public MessageMediaDocumentConstructor()
        {

        }

        public MessageMediaDocumentConstructor(Document document)
        {
            this.document = document;
        }


        public override Constructor constructor => Constructor.MessageMediaDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2fda2204);
            document.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            document = Read<Document>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaDocument document:{document})";
        }
    }


    public class MessageMediaAudioConstructor : MessageMedia
    {
        public Audio audio;

        public MessageMediaAudioConstructor()
        {

        }

        public MessageMediaAudioConstructor(Audio audio)
        {
            this.audio = audio;
        }


        public override Constructor constructor => Constructor.MessageMediaAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc6b68300);
            audio.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            audio = Read<Audio>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaAudio audio:{audio})";
        }
    }


    public class InputAudioEmptyConstructor : InputAudio
    {
        public override Constructor constructor => Constructor.InputAudioEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd95adc84);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputAudioEmpty)";
        }
    }


    public class InputAudioConstructor : InputAudio
    {
        public long id;
        public long accessHash;

        public InputAudioConstructor()
        {

        }

        public InputAudioConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x77d440ff);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputAudio id:{id} accessHash:{accessHash})";
        }
    }


    public class InputDocumentEmptyConstructor : InputDocument
    {
        public override Constructor constructor => Constructor.InputDocumentEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x72f0eaae);
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override string ToString()
        {
            return "(inputDocumentEmpty)";
        }
    }


    public class InputDocumentConstructor : InputDocument
    {
        public long id;
        public long accessHash;

        public InputDocumentConstructor()
        {

        }

        public InputDocumentConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x18798952);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputDocument id:{id} accessHash:{accessHash})";
        }
    }


    public class InputAudioFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long accessHash;

        public InputAudioFileLocationConstructor()
        {

        }

        public InputAudioFileLocationConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputAudioFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x74dc404d);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputAudioFileLocation id:{id} accessHash:{accessHash})";
        }
    }


    public class InputDocumentFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long accessHash;

        public InputDocumentFileLocationConstructor()
        {

        }

        public InputDocumentFileLocationConstructor(long id, long accessHash)
        {
            this.id = id;
            this.accessHash = accessHash;
        }


        public override Constructor constructor => Constructor.InputDocumentFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4e45abe9);
            writer.Write(id);
            writer.Write(accessHash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputDocumentFileLocation id:{id} accessHash:{accessHash})";
        }
    }


    public class DecryptedMessageMediaDocumentConstructor : DecryptedMessageMedia
    {
        public byte[] thumb;
        public int thumbW;
        public int thumbH;
        public string fileName;
        public string mimeType;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaDocumentConstructor()
        {

        }

        public DecryptedMessageMediaDocumentConstructor(byte[] thumb, int thumbW, int thumbH, string fileName,
            string mimeType, int size, byte[] key, byte[] iv)
        {
            this.thumb = thumb;
            this.thumbW = thumbW;
            this.thumbH = thumbH;
            this.fileName = fileName;
            this.mimeType = mimeType;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor constructor => Constructor.DecryptedMessageMediaDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb095434b);
            Serializers.Bytes.Write(writer, thumb);
            writer.Write(thumbW);
            writer.Write(thumbH);
            Serializers.String.Write(writer, fileName);
            Serializers.String.Write(writer, mimeType);
            writer.Write(size);
            Serializers.Bytes.Write(writer, key);
            Serializers.Bytes.Write(writer, iv);
        }

        public override void Read(BinaryReader reader)
        {
            thumb = Serializers.Bytes.Read(reader);
            thumbW = reader.ReadInt32();
            thumbH = reader.ReadInt32();
            fileName = Serializers.String.Read(reader);
            mimeType = Serializers.String.Read(reader);
            size = reader.ReadInt32();
            key = Serializers.Bytes.Read(reader);
            iv = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaDocument thumb:{BitConverter.ToString(thumb)} thumbW:{thumbW} thumbH:{thumbH} fileName:'{fileName}' mimeType:'{mimeType}' size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
        }
    }


    public class DecryptedMessageMediaAudioConstructor : DecryptedMessageMedia
    {
        public int duration;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaAudioConstructor()
        {

        }

        public DecryptedMessageMediaAudioConstructor(int duration, int size, byte[] key, byte[] iv)
        {
            this.duration = duration;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor constructor => Constructor.DecryptedMessageMediaAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6080758f);
            writer.Write(duration);
            writer.Write(size);
            Serializers.Bytes.Write(writer, key);
            Serializers.Bytes.Write(writer, iv);
        }

        public override void Read(BinaryReader reader)
        {
            duration = reader.ReadInt32();
            size = reader.ReadInt32();
            key = Serializers.Bytes.Read(reader);
            iv = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaAudio duration:{duration} size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
        }
    }


    public class AudioEmptyConstructor : Audio
    {
        public long id;

        public AudioEmptyConstructor()
        {

        }

        public AudioEmptyConstructor(long id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.AudioEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x586988d8);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(audioEmpty id:{id})";
        }
    }


    public class AudioConstructor : Audio
    {
        public long id;
        public long accessHash;
        public int userId;
        public int date;
        public int duration;
        public string mimeType;
        public int size;
        public int dcId;

        public AudioConstructor()
        {

        }

        public AudioConstructor(long id, long accessHash, int userId, int date, int duration, string mimeType, int size, int dcId)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.userId = userId;
            this.date = date;
            this.duration = duration;
            this.mimeType = mimeType;
            this.size = size;
            this.dcId = dcId;
        }


        public override Constructor constructor => Constructor.Audio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc7ac6496);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(userId);
            writer.Write(date);
            writer.Write(duration);
            Serializers.String.Write(writer, mimeType);
            writer.Write(size);
            writer.Write(dcId);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
            userId = reader.ReadInt32();
            date = reader.ReadInt32();
            duration = reader.ReadInt32();
            mimeType = Serializers.String.Read(reader);
            size = reader.ReadInt32();
            dcId = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(audio id:{id} accessHash:{accessHash} userId:{userId} date:{date} duration:{duration} mimeType:{mimeType} size:{size} dc_id:{dcId})";
        }
    }


    public class DocumentEmptyConstructor : Document
    {
        public long id;

        public DocumentEmptyConstructor()
        {

        }

        public DocumentEmptyConstructor(long id)
        {
            this.id = id;
        }


        public override Constructor constructor => Constructor.DocumentEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x36f8c871);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(documentEmpty id:{id})";
        }
    }


    public class DocumentConstructor : Document
    {
        public long id;
        public long accessHash;
        public int date;
        public string mimeType;
        public int size;
        public PhotoSize thumb;
        public int dcId;
        public List<DocumentAttribute> attributes;

        public DocumentConstructor()
        {

        }

        public DocumentConstructor(long id, long accessHash, int date, string mimeType,
            int size, PhotoSize thumb, int dcId, List<DocumentAttribute> attributes)
        {
            this.id = id;
            this.accessHash = accessHash;
            this.date = date;
            this.mimeType = mimeType;
            this.size = size;
            this.thumb = thumb;
            this.dcId = dcId;
            this.attributes = attributes;
        }


        public override Constructor constructor => Constructor.Document;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf9a39f4f);
            writer.Write(id);
            writer.Write(accessHash);
            writer.Write(date);
            Serializers.String.Write(writer, mimeType);
            writer.Write(size);
            thumb.Write(writer);
            writer.Write(dcId);

            writer.Write(0x1cb5c415);
            writer.Write(attributes.Count);
            foreach (DocumentAttribute attributeElement in attributes)
            {
                attributeElement.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            accessHash = reader.ReadInt64();
            date = reader.ReadInt32();
            mimeType = Serializers.String.Read(reader);
            size = reader.ReadInt32();
            thumb = Read<PhotoSize>(reader);
            dcId = reader.ReadInt32();

            reader.ReadUInt32(); // vector code
            int attributesLen = reader.ReadInt32();
            attributes = new List<DocumentAttribute>(attributesLen);
            for (int attributesIndex = 0; attributesIndex < attributesLen; attributesIndex++)
            {
                var attributeElement = Read<DocumentAttribute>(reader);
                attributes.Add(attributeElement);
            }
        }

        public override string ToString()
        {
            return
                $"(document id:{id} accessHash:{accessHash} date:{date} mime_type:'{mimeType}' size:{size} thumb:{thumb} dc_id:{dcId})";
        }
    }

    public abstract class DocumentAttribute : TLObject
    {
    }

    public class DocumentAttributeImageSizeConstructor : DocumentAttribute
    {
        public int w;
        public int h;

        public DocumentAttributeImageSizeConstructor()
        {
        }

        public DocumentAttributeImageSizeConstructor(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public override Constructor constructor => Constructor.DocumentAttributeImageSize;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6c37c15c);
            writer.Write(w);
            writer.Write(h);
        }

        public override void Read(BinaryReader reader)
        {
            w = reader.ReadInt32();
            h = reader.ReadInt32();
        }
    }

    public class DocumentAttributeAnimatedConstructor : DocumentAttribute
    {
        public override Constructor constructor => Constructor.DocumentAttributeAnimated;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x11b58939);
        }

        public override void Read(BinaryReader reader)
        {
        }
    }

    public class DocumentAttributeStickerConstructor : DocumentAttribute
    {
        public override Constructor constructor => Constructor.DocumentAttributeSticker;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfb0a5727);
        }

        public override void Read(BinaryReader reader)
        {
        }
    }

    public class DocumentAttributeVideoConstructor : DocumentAttribute
    {
        public int duration;
        public int w;
        public int h;

        public DocumentAttributeVideoConstructor()
        {
        }

        public DocumentAttributeVideoConstructor(int duration, int w, int h)
        {
            this.duration = duration;
            this.w = w;
            this.h = h;
        }

        public override Constructor constructor => Constructor.DocumentAttributeVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5910cccb);
            writer.Write(duration);
            writer.Write(w);
            writer.Write(h);
        }

        public override void Read(BinaryReader reader)
        {
            duration = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
        }
    }

    public class DocumentAttributeAudioConstructor : DocumentAttribute
    {
        public int duration;

        public DocumentAttributeAudioConstructor()
        {
        }

        public DocumentAttributeAudioConstructor(int duration)
        {
            this.duration = duration;
        }

        public override Constructor constructor => Constructor.DocumentAttributeAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x51448e5);
            writer.Write(duration);
        }

        public override void Read(BinaryReader reader)
        {
            duration = reader.ReadInt32();
        }
    }

    public class DocumentAttributeFilenameConstructor : DocumentAttribute
    {
        public string fileName;

        public DocumentAttributeFilenameConstructor()
        {
        }

        public DocumentAttributeFilenameConstructor(string fileName)
        {
            this.fileName = fileName;
        }

        public override Constructor constructor => Constructor.DocumentAttributeFilename;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x15590068);
            writer.Write(fileName);
        }

        public override void Read(BinaryReader reader)
        {
            fileName = Serializers.String.Read(reader);
        }
    }

    public class DisabledFeatureConstructor : TLObject
    {
        public string feature;
        public string description;

        public DisabledFeatureConstructor()
        {
        }

        public DisabledFeatureConstructor(string feature, string description)
        {
            this.feature = feature;
            this.description = description;
        }

        public override Constructor constructor => Constructor.DisabledFeature;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xae636f24);
            Serializers.String.Write(writer, feature);
            Serializers.String.Write(writer, description);
        }

        public override void Read(BinaryReader reader)
        {
            feature = Serializers.String.Read(reader);
            description = Serializers.String.Read(reader);
        }
    }

    public class Pong : TLObject
    {
        public long messageId;
        public long pingId;

        public override Constructor constructor => Constructor.Pong;
        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x347773c5);
            writer.Write(messageId);
            writer.Write(pingId);
        }

        public override void Read(BinaryReader reader)
        {
            messageId = reader.ReadInt64();
            pingId = reader.ReadInt64();
        }
    }

    public enum SendMessageAction : uint
    {
        TypingAction = 0x16bf744e,
        CancelAction = 0xfd5ec8f5,
        RecordVideoAction = 0xa187d66f,
        UploadVideoAction = 0x92042ff7,
        RecordAudioAction = 0xd52f73f7,
        UploadAudioAction = 0xe6ac8a6f,
        UploadPhotoAction = 0x990a3c1a,
        UploadDocumentAction = 0x8faee98e,
        GeoLocationAction = 0x176f8ba1,
        ChooseContactAction = 0x628cbc6f,
    }

    public enum RpcRequestError
    {
        None = 0,

        // Message level errors

        MessageIdTooLow = 16,           // msg_id too low (most likely, client time is wrong; it would be worthwhile to synchronize it using msg_id notifications and re-send the original message with the “correct” msg_id or wrap it in a container with a new msg_id if the original message had waited too long on the client to be transmitted)
        MessageIdTooHigh,               // msg_id too high (similar to the previous case, the client time has to be synchronized, and the message re-sent with the correct msg_id)
        CorruptedMessageId,             // incorrect two lower order msg_id bits (the server expects client message msg_id to be divisible by 4)
        DuplicateOfMessageContainerId,  // container msg_id is the same as msg_id of a previously received message (this must never happen)
        MessageTooOld,                  // message too old, and it cannot be verified whether the server has received a message with this msg_id or not

        MessageSeqNoTooLow = 32,        // msg_seqno too low (the server has already received a message with a lower msg_id but with either a higher or an equal and odd seqno)
        MessageSeqNoTooHigh,            // msg_seqno too high (similarly, there is a message with a higher msg_id but with either a lower or an equal and odd seqno)
        EvenSeqNoExpected,              // an even msg_seqno expected (irrelevant message), but odd received
        OddSeqNoExpected,               // odd msg_seqno expected (relevant message), but even received

        IncorrectServerSalt = 48,       // incorrect server salt (in this case, the bad_server_salt response is received with the correct salt, and the message is to be re-sent with it)
        InvalidContainer = 64,           // invalid container

        // Api-request level errors

        MigrateDataCenter = 303,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Flood = 420,
        InternalServer = 500
    }

    public enum VerificationCodeDeliveryType
    {
        NumericCodeViaSms = 0,
        NumericCodeViaTelegram = 5
    }
}