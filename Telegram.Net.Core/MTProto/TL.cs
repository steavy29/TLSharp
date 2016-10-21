using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Telegram.Net.Core.MTProto
{

    public abstract class TLObject
    {
        public abstract Constructor Constructor { get; }
        public abstract void Write(BinaryWriter writer);
        public abstract void Read(BinaryReader reader);
    }

    // all constructor types

    public enum Constructor
    {
        messageUndelivered,
        error,
        inputPeerEmpty,
        inputPeerSelf,
        inputPeerContact,
        inputPeerForeign,
        inputPeerChat,
        inputUserEmpty,
        inputUserSelf,
        inputUserContact,
        inputUserForeign,
        inputPhoneContact,
        inputFile,
        inputMediaEmpty,
        inputMediaUploadedPhoto,
        inputMediaPhoto,
        inputMediaGeoPoint,
        inputMediaContact,
        inputMediaUploadedVideo,
        inputMediaUploadedThumbVideo,
        inputMediaVideo,
        inputChatPhotoEmpty,
        inputChatUploadedPhoto,
        inputChatPhoto,
        inputGeoPointEmpty,
        inputGeoPoint,
        inputPhotoEmpty,
        inputPhoto,
        inputVideoEmpty,
        inputVideo,
        inputFileLocation,
        inputVideoFileLocation,
        inputPhotoCropAuto,
        inputPhotoCrop,
        inputAppEvent,
        peerUser,
        peerChat,
        storage_fileUnknown,
        storage_fileJpeg,
        storage_fileGif,
        storage_filePng,
        storage_fileMp3,
        storage_fileMov,
        storage_filePartial,
        storage_fileMp4,
        storage_fileWebp,
        fileLocationUnavailable,
        fileLocation,
        userEmpty,
        userSelf,
        userContact,
        userRequest,
        userForeign,
        userDeleted,
        userProfilePhotoEmpty,
        userProfilePhoto,
        userStatusEmpty,
        userStatusOnline,
        userStatusOffline,
        chatEmpty,
        chat,
        chatForbidden,
        chatFull,
        chatParticipant,
        chatParticipantsForbidden,
        chatParticipants,
        chatPhotoEmpty,
        chatPhoto,
        messageEmpty,
        message,
        messageForwarded,
        messageService,
        messageMediaEmpty,
        messageMediaPhoto,
        messageMediaVideo,
        messageMediaGeo,
        messageMediaContact,
        messageMediaUnsupported,
        messageActionEmpty,
        messageActionChatCreate,
        messageActionChatEditTitle,
        messageActionChatEditPhoto,
        messageActionChatDeletePhoto,
        messageActionChatAddUser,
        messageActionChatDeleteUser,
        dialog,
        photoEmpty,
        photo,
        photoSizeEmpty,
        photoSize,
        photoCachedSize,
        videoEmpty,
        video,
        geoPointEmpty,
        geoPoint,
        auth_checkedPhone,
        authSentCode,
        authSentAppCode,
        auth_authorization,
        auth_exportedAuthorization,
        inputNotifyPeer,
        inputNotifyUsers,
        inputNotifyChats,
        inputNotifyAll,
        inputPeerNotifyEventsEmpty,
        inputPeerNotifyEventsAll,
        inputPeerNotifySettings,
        peerNotifyEventsEmpty,
        peerNotifyEventsAll,
        peerNotifySettingsEmpty,
        peerNotifySettings,
        wallPaper,
        userFull,
        contact,
        importedContact,
        contactBlocked,
        contactFound,
        contactSuggested,
        contactStatus,
        chatLocated,
        contacts_foreignLinkUnknown,
        contacts_foreignLinkRequested,
        contacts_foreignLinkMutual,
        contacts_myLinkEmpty,
        contacts_myLinkRequested,
        contacts_myLinkContact,
        contacts_link,
        contacts_contacts,
        contacts_contactsNotModified,
        contacts_importedContacts,
        contacts_blocked,
        contacts_blockedSlice,
        contacts_found,
        contacts_suggested,
        messages_dialogs,
        messages_dialogsSlice,
        messages_messages,
        messages_messagesSlice,
        messages_messageEmpty,
        messages_message,
        messages_statedMessages,
        messages_statedMessage,
        messages_sentMessage,
        messages_chat,
        messages_chats,
        messages_chatFull,
        messages_affectedHistory,
        inputMessagesFilterEmpty,
        inputMessagesFilterPhotos,
        inputMessagesFilterVideo,
        inputMessagesFilterPhotoVideo,
        updateNewMessage,
        updateMessageID,
        updateReadMessages,
        updateDeleteMessages,
        updateRestoreMessages,
        updateUserTyping,
        updateChatUserTyping,
        updateChatParticipants,
        updateUserStatus,
        updateUserName,
        updateUserPhoto,
        updateContactRegistered,
        updateContactLink,
        updateActivation,
        updateNewAuthorization,
        updates_state,
        updates_differenceEmpty,
        updates_difference,
        updates_differenceSlice,
        updatesTooLong,
        updateShortMessage,
        updateShortChatMessage,
        updateShort,
        updatesCombined,
        updates,
        photos_photos,
        photos_photosSlice,
        photos_photo,
        upload_file,
        dcOption,
        config,
        nearestDc,
        help_appUpdate,
        help_noAppUpdate,
        help_inviteText,
        messages_statedMessagesLinks,
        messages_statedMessageLink,
        messages_sentMessageLink,
        inputGeoChat,
        inputNotifyGeoChatPeer,
        geoChat,
        geoChatMessageEmpty,
        geoChatMessage,
        geoChatMessageService,
        geochats_statedMessage,
        geochats_located,
        geochats_messages,
        geochats_messagesSlice,
        messageActionGeoChatCreate,
        messageActionGeoChatCheckin,
        updateNewGeoChatMessage,
        wallPaperSolid,
        updateNewEncryptedMessage,
        updateEncryptedChatTyping,
        updateEncryption,
        updateEncryptedMessagesRead,
        encryptedChatEmpty,
        encryptedChatWaiting,
        encryptedChatRequested,
        encryptedChat,
        encryptedChatDiscarded,
        inputEncryptedChat,
        encryptedFileEmpty,
        encryptedFile,
        inputEncryptedFileEmpty,
        inputEncryptedFileUploaded,
        inputEncryptedFile,
        inputEncryptedFileLocation,
        encryptedMessage,
        encryptedMessageService,
        decryptedMessageLayer,
        decryptedMessage,
        decryptedMessageService,
        decryptedMessageMediaEmpty,
        decryptedMessageMediaPhoto,
        decryptedMessageMediaVideo,
        decryptedMessageMediaGeoPoint,
        decryptedMessageMediaContact,
        decryptedMessageActionSetMessageTTL,
        messages_dhConfigNotModified,
        messages_dhConfig,
        messages_sentEncryptedMessage,
        messages_sentEncryptedFile,
        inputFileBig,
        inputEncryptedFileBigUploaded,
        updateChatParticipantAdd,
        updateChatParticipantDelete,
        updateDcOptions,
        updateUserBlocked,
        updateNotifySettings,
        updateServiceNotification,
        updatePrivacy,
        updateUserPhone,
        inputMediaUploadedAudio,
        inputMediaAudio,
        inputMediaUploadedDocument,
        inputMediaUploadedThumbDocument,
        inputMediaDocument,
        messageMediaDocument,
        messageMediaAudio,
        inputAudioEmpty,
        inputAudio,
        inputDocumentEmpty,
        inputDocument,
        inputAudioFileLocation,
        inputDocumentFileLocation,
        decryptedMessageMediaDocument,
        decryptedMessageMediaAudio,
        audioEmpty,
        audio,
        documentEmpty,
        document,
        documentAttributeImageSize,
        documentAttributeAnimated,
        documentAttributeSticker,
        documentAttributeVideo,
        documentAttributeAudio,
        documentAttributeFilename
    }

    public class TL
    {
        private static readonly Dictionary<uint, Type> constructors = new Dictionary<uint, Type>
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
            {0xaa963b05, typeof (Storage_fileUnknownConstructor)},
            {0x007efe0e, typeof (Storage_fileJpegConstructor)},
            {0xcae1aadf, typeof (Storage_fileGifConstructor)},
            {0x0a4f63c0, typeof (Storage_filePngConstructor)},
            {0x528a0677, typeof (Storage_fileMp3Constructor)},
            {0x4b09ebbc, typeof (Storage_fileMovConstructor)},
            {0x40bc6f52, typeof (Storage_filePartialConstructor)},
            {0xb3cea0e4, typeof (Storage_fileMp4Constructor)},
            {0x1081464c, typeof (Storage_fileWebpConstructor)},
            {0x7c596b46, typeof (FileLocationUnavailableConstructor)},
            {0x53d69076, typeof (FileLocationConstructor)},
            {0x200250ba, typeof (UserEmptyConstructor)},
            {0x720535EC, typeof (UserSelfConstructor)},
            {0x7007b451, typeof (UserSelfConstructor)},
            {0xcab35e18, typeof (UserContactConstructor)}, //before signed as 0xf2fb8319
            {0x22e8ceb0, typeof (UserRequestConstructor)},
            {0x5214c89d, typeof (UserForeignConstructor)},
            {0xb29ad7cc, typeof (UserDeletedConstructor)},
            {0x4f11bae1, typeof (UserProfilePhotoEmptyConstructor)},
            {0xd559d8c8, typeof (UserProfilePhotoConstructor)},
            {0x09d05049, typeof (UserStatusEmptyConstructor)},
            {0xedb93949, typeof (UserStatusOnlineConstructor)},
            {0x008c703f, typeof (UserStatusOfflineConstructor)},
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
            {0x5a04a49f, typeof (VideoConstructor)},
            {0x1117dd5f, typeof (GeoPointEmptyConstructor)},
            {0x2049d70c, typeof (GeoPointConstructor)},
            {0xe300cc3b, typeof (Auth_checkedPhoneConstructor)},
            {0xefed51d9, typeof (AuthSentCodeConstructor)},
            {0xe325edcf, typeof (AuthSentAppCodeConstructor)},
            {0xf6b673a4, typeof (Auth_authorizationConstructor)},
            {0xdf969c2d, typeof (Auth_exportedAuthorizationConstructor)},
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
            {0x133421f8, typeof (Contacts_foreignLinkUnknownConstructor)},
            {0xa7801f47, typeof (Contacts_foreignLinkRequestedConstructor)},
            {0x1bea8ce1, typeof (Contacts_foreignLinkMutualConstructor)},
            {0xd22a1c60, typeof (Contacts_myLinkEmptyConstructor)},
            {0x6c69efee, typeof (Contacts_myLinkRequestedConstructor)},
            {0xc240ebd9, typeof (Contacts_myLinkContactConstructor)},
            {0xeccea3f5, typeof (Contacts_linkConstructor)},
            {0xb74ba9d2, typeof (Contacts_contactsNotModifiedConstructor)},
            {0x6f8b8cb2, typeof (Contacts_contactsConstructor)},
            {0xad524315, typeof (ContactsImportedContactsConstructor)},
            {0x1c138d15, typeof (Contacts_blockedConstructor)},
            {0x900802a1, typeof (Contacts_blockedSliceConstructor)},
            {0x0566000e, typeof (Contacts_foundConstructor)},
            {0x5649dcc5, typeof (Contacts_suggestedConstructor)},
            {0x15ba6c40, typeof (Messages_dialogsConstructor)},
            {0x71e094f3, typeof (Messages_dialogsSliceConstructor)},
            {0x8c718e87, typeof (Messages_messagesConstructor)},
            {0x0b446ae3, typeof (Messages_messagesSliceConstructor)},
            {0x3f4e0648, typeof (Messages_messageEmptyConstructor)},
            {0xff90c417, typeof (Messages_messageConstructor)},
            {0x969478bb, typeof (Messages_statedMessagesConstructor)},
            {0xd07ae726, typeof (Messages_statedMessageConstructor)},
            {0xd1f4d35c, typeof (SentMessageConstructor)},
            {0x40e9002a, typeof (Messages_chatConstructor)},
            {0x8150cbd8, typeof (Messages_chatsConstructor)},
            {0xe5d7d19c, typeof (Messages_chatFullConstructor)},
            {0xb7de36f2, typeof (Messages_affectedHistoryConstructor)},
            {0x57e2f66c, typeof (InputMessagesFilterEmptyConstructor)},
            {0x9609a51c, typeof (InputMessagesFilterPhotosConstructor)},
            {0x9fc00e65, typeof (InputMessagesFilterVideoConstructor)},
            {0x56e9f0e4, typeof (InputMessagesFilterPhotoVideoConstructor)},
            {0x013abdb3, typeof (UpdateNewMessageConstructor)},
            {0x4e90bfd6, typeof (UpdateMessageIDConstructor)},
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
            {0xa56c2a3e, typeof (Updates_stateConstructor)},
            {0x5d75a138, typeof (Updates_differenceEmptyConstructor)},
            {0x00f49ca0, typeof (Updates_differenceConstructor)},
            {0xa8fb1981, typeof (Updates_differenceSliceConstructor)},
            {0xe317af7e, typeof (UpdatesTooLongConstructor)},
            {0xd3f45784, typeof (UpdateShortMessageConstructor)},
            {0x2b2fbd4e, typeof (UpdateShortChatMessageConstructor)},
            {0x78d4dec1, typeof (UpdateShortConstructor)},
            {0x725b04c3, typeof (UpdatesCombinedConstructor)},
            {0x74ae4240, typeof (UpdatesConstructor)},
            {0x8dca6aa5, typeof (Photos_photosConstructor)},
            {0x15051f54, typeof (Photos_photosSliceConstructor)},
            {0x20212ca8, typeof (Photos_photoConstructor)},
            {0x096a18d5, typeof (Upload_fileConstructor)},
            {0x2ec2a43c, typeof (DcOptionConstructor)},
            {0x232d5905, typeof (ConfigConstructor)},
            {0x8e1a1775, typeof (NearestDcConstructor)},
            {0x8987f311, typeof (Help_appUpdateConstructor)},
            {0xc45a6536, typeof (Help_noAppUpdateConstructor)},
            {0x18cb9f78, typeof (Help_inviteTextConstructor)},
            {0x3e74f5c6, typeof (Messages_statedMessagesLinksConstructor)},
            {0xa9af2881, typeof (Messages_statedMessageLinkConstructor)},
            {0xe9db4a3f, typeof (SentMessageLinkConstructor)},
            {0x74d456fa, typeof (InputGeoChatConstructor)},
            {0x4d8ddec8, typeof (InputNotifyGeoChatPeerConstructor)},
            {0x75eaea5a, typeof (GeoChatConstructor)},
            {0x60311a9b, typeof (GeoChatMessageEmptyConstructor)},
            {0x4505f8e1, typeof (GeoChatMessageConstructor)},
            {0xd34fa24e, typeof (GeoChatMessageServiceConstructor)},
            {0x17b1578b, typeof (Geochats_statedMessageConstructor)},
            {0x48feb267, typeof (Geochats_locatedConstructor)},
            {0xd1526db1, typeof (Geochats_messagesConstructor)},
            {0xbc5863e8, typeof (Geochats_messagesSliceConstructor)},
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
            {0xa1733aec, typeof (DecryptedMessageActionSetMessageTTLConstructor)},
            {0xc0e24635, typeof (Messages_dhConfigNotModifiedConstructor)},
            {0x2c221edd, typeof (Messages_dhConfigConstructor)},
            {0x560f8935, typeof (Messages_sentEncryptedMessageConstructor)},
            {0x9493ff32, typeof (Messages_sentEncryptedFileConstructor)},
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
            {0x427425e7, typeof (AudioConstructor)},
            {0x36f8c871, typeof (DocumentEmptyConstructor)},
            {0xf9a39f4f, typeof (DocumentConstructor)},
            {0xab3a99ac, typeof (DialogConstructor)},
            {0xd9ccc4ef, typeof (UserRequestConstructor)},
            {0x075cf7a8, typeof (UserForeignConstructor)},
            {0x6c37c15c, typeof (DocumentAttributeImageSizeConstructor)},
            {0x11b58939, typeof (DocumentAttributeAnimatedConstructor)},
            {0xfb0a5727, typeof (DocumentAttributeStickerConstructor)},
            {0x5910cccb, typeof (DocumentAttributeVideoConstructor)},
            {0x51448e5, typeof (DocumentAttributeAudioConstructor)},
            {0x15590068, typeof (DocumentAttributeFilenameConstructor)}
        };

        public static T Parse<T>(BinaryReader reader)
        {
            uint dataCode = reader.ReadUInt32();
            return Parse<T>(reader, dataCode);
        }

        public static T Parse<T>(BinaryReader reader, uint dataCode)
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

        public static List<T> ParseVector<T>(BinaryReader reader, bool readVectorDataCode = true) where T : TLObject
        {
            return ParseVector(reader, () => Parse<T>(reader), readVectorDataCode);
        }

        public static List<T> ParseVector<T>(BinaryReader reader, Func<T> readNextFunc, bool readVectorDataCode = true)
        {
            if (readVectorDataCode)
                reader.ReadInt32(); // vector code

            int count = reader.ReadInt32();
            var result = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(readNextFunc());
            }

            return result;
        }
    }

    // abstract types
    public abstract class contacts_ImportedContacts : TLObject
    {

    }

    public abstract class Peer : TLObject
    {

    }

    public abstract class InputVideo : TLObject
    {

    }

    public abstract class help_InviteText : TLObject
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

    public abstract class help_AppUpdate : TLObject
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

    public abstract class messages_SentEncryptedMessage : TLObject
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

    public abstract class messages_ChatFull : TLObject
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

    public abstract class contacts_ForeignLink : TLObject
    {

    }

    public abstract class Document : TLObject
    {

    }

    public abstract class UserFull : TLObject
    {

    }

    public abstract class messages_Message : TLObject
    {

    }

    public abstract class DcOption : TLObject
    {

    }

    public abstract class photos_Photos : TLObject
    {

    }

    public abstract class InputPeerNotifySettings : TLObject
    {

    }

    public abstract class contacts_Suggested : TLObject
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

    public abstract class auth_Authorization : TLObject
    {

    }

    public abstract class auth_CheckedPhone : TLObject
    {

    }

    public abstract class FileLocation : TLObject
    {

    }

    public abstract class messages_Chats : TLObject
    {

    }

    public abstract class contacts_Link : TLObject
    {

    }

    public abstract class messages_StatedMessage : TLObject
    {

    }

    public abstract class geochats_Located : TLObject
    {

    }

    public abstract class updates_State : TLObject
    {

    }

    public abstract class storage_FileType : TLObject
    {

    }

    public abstract class geochats_StatedMessage : TLObject
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

    public abstract class photos_Photo : TLObject
    {

    }

    public abstract class InputFile : TLObject
    {

    }

    public abstract class auth_ExportedAuthorization : TLObject
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

    public abstract class contacts_Blocked : TLObject
    {

    }

    public abstract class messages_AffectedHistory : TLObject
    {

    }

    public abstract class messages_Chat : TLObject
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

    public abstract class messages_Messages : TLObject
    {

    }

    public abstract class messages_Dialogs : TLObject
    {

    }

    public abstract class InputPeer : TLObject
    {

    }

    public abstract class ChatPhoto : TLObject
    {

    }

    public abstract class contacts_MyLink : TLObject
    {

    }

    public abstract class InputChatPhoto : TLObject
    {

    }

    public abstract class SentMessage : TLObject
    {

    }

    public abstract class messages_StatedMessages : TLObject
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

    public abstract class geochats_Messages : TLObject
    {

    }

    public abstract class InputUser : TLObject
    {

    }

    public abstract class EncryptedChat : TLObject
    {

    }

    public abstract class contacts_Contacts : TLObject
    {

    }

    public abstract class GeoChatMessage : TLObject
    {

    }

    public abstract class PeerNotifyEvents : TLObject
    {

    }

    public abstract class contacts_Found : TLObject
    {

    }

    public abstract class Audio : TLObject
    {

    }

    public abstract class ChatFull : TLObject
    {

    }

    public abstract class messages_DhConfig : TLObject
    {

    }

    public abstract class Contact : TLObject
    {

    }

    public abstract class upload_File : TLObject
    {

    }

    public abstract class InputPhotoCrop : TLObject
    {

    }

    public abstract class ContactSuggested : TLObject
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


        public override Constructor Constructor => Constructor.error;

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

        public InputPeerEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputPeerEmpty;

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

        public InputPeerSelfConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputPeerSelf;

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
        public int user_id;

        public InputPeerContactConstructor()
        {

        }

        public InputPeerContactConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.inputPeerContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1023dbe8);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputPeerContact user_id:{user_id})";
        }
    }


    public class InputPeerForeignConstructor : InputPeer
    {
        public int user_id;
        public long access_hash;

        public InputPeerForeignConstructor()
        {

        }

        public InputPeerForeignConstructor(int user_id, long access_hash)
        {
            this.user_id = user_id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputPeerForeign;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9b447325);
            writer.Write(user_id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputPeerForeign user_id:{user_id} access_hash:{access_hash})";
        }
    }


    public class InputPeerChatConstructor : InputPeer
    {
        public int chat_id;

        public InputPeerChatConstructor()
        {

        }

        public InputPeerChatConstructor(int chat_id)
        {
            this.chat_id = chat_id;
        }


        public override Constructor Constructor => Constructor.inputPeerChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x179be863);
            writer.Write(chat_id);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputPeerChat chat_id:{chat_id})";
        }
    }


    public class InputUserEmptyConstructor : InputUser
    {

        public InputUserEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputUserEmpty;

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

        public InputUserSelfConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputUserSelf;

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
        public int user_id;

        public InputUserContactConstructor()
        {

        }

        public InputUserContactConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.inputUserContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x86e94f65);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputUserContact user_id:{user_id})";
        }
    }


    public class InputUserForeignConstructor : InputUser
    {
        public int user_id;
        public long access_hash;

        public InputUserForeignConstructor()
        {

        }

        public InputUserForeignConstructor(int user_id, long access_hash)
        {
            this.user_id = user_id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputUserForeign;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x655e74ff);
            writer.Write(user_id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputUserForeign user_id:{user_id} access_hash:{access_hash})";
        }
    }


    public class InputPhoneContactConstructor : InputContact
    {
        public long client_id;
        public string phone;
        public string first_name;
        public string last_name;

        public InputPhoneContactConstructor()
        {

        }

        public InputPhoneContactConstructor(long client_id, string phone, string first_name, string last_name)
        {
            this.client_id = client_id;
            this.phone = phone;
            this.first_name = first_name;
            this.last_name = last_name;
        }


        public override Constructor Constructor => Constructor.inputPhoneContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf392b7f4);
            writer.Write(client_id);
            Serializers.String.Write(writer, phone);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
        }

        public override void Read(BinaryReader reader)
        {
            client_id = reader.ReadInt64();
            phone = Serializers.String.Read(reader);
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(inputPhoneContact client_id:{client_id} phone:'{phone}' first_name:'{first_name}' last_name:'{last_name}')";
        }
    }


    public class InputFileConstructor : InputFile
    {
        public long id;
        public int parts;
        public string name;
        public string md5_checksum;

        public InputFileConstructor()
        {

        }

        public InputFileConstructor(long id, int parts, string name, string md5_checksum)
        {
            this.id = id;
            this.parts = parts;
            this.name = name;
            this.md5_checksum = md5_checksum;
        }


        public override Constructor Constructor => Constructor.inputFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf52ff27f);
            writer.Write(id);
            writer.Write(parts);
            Serializers.String.Write(writer, name);
            Serializers.String.Write(writer, md5_checksum);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            name = Serializers.String.Read(reader);
            md5_checksum = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(inputFile id:{id} parts:{parts} name:'{name}' md5_checksum:'{md5_checksum}')";
        }
    }


    public class InputMediaEmptyConstructor : InputMedia
    {

        public InputMediaEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputMediaEmpty;

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


        public override Constructor Constructor => Constructor.inputMediaUploadedPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2dc53a7d);
            file.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            file = TL.Parse<InputFile>(reader);
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


        public override Constructor Constructor => Constructor.inputMediaPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8f2ab2ec);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = TL.Parse<InputPhoto>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaPhoto id:{id})";
        }
    }


    public class InputMediaGeoPointConstructor : InputMedia
    {
        public InputGeoPoint geo_point;

        public InputMediaGeoPointConstructor()
        {

        }

        public InputMediaGeoPointConstructor(InputGeoPoint geo_point)
        {
            this.geo_point = geo_point;
        }


        public override Constructor Constructor => Constructor.inputMediaGeoPoint;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf9c44144);
            geo_point.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            geo_point = TL.Parse<InputGeoPoint>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaGeoPoint geo_point:{geo_point})";
        }
    }


    public class InputMediaContactConstructor : InputMedia
    {
        public string phone_number;
        public string first_name;
        public string last_name;

        public InputMediaContactConstructor()
        {

        }

        public InputMediaContactConstructor(string phone_number, string first_name, string last_name)
        {
            this.phone_number = phone_number;
            this.first_name = first_name;
            this.last_name = last_name;
        }


        public override Constructor Constructor => Constructor.inputMediaContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa6e45987);
            Serializers.String.Write(writer, phone_number);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
        }

        public override void Read(BinaryReader reader)
        {
            phone_number = Serializers.String.Read(reader);
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(inputMediaContact phone_number:'{phone_number}' first_name:'{first_name}' last_name:'{last_name}')";
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


        public override Constructor Constructor => Constructor.inputMediaUploadedVideo;

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
            file = TL.Parse<InputFile>(reader);
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


        public override Constructor Constructor => Constructor.inputMediaUploadedThumbVideo;

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
            file = TL.Parse<InputFile>(reader);
            thumb = TL.Parse<InputFile>(reader);
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


        public override Constructor Constructor => Constructor.inputMediaVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7f023ae6);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = TL.Parse<InputVideo>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaVideo id:{id})";
        }
    }


    public class InputChatPhotoEmptyConstructor : InputChatPhoto
    {

        public InputChatPhotoEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputChatPhotoEmpty;

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


        public override Constructor Constructor => Constructor.inputChatUploadedPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x94254732);
            file.Write(writer);
            crop.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            file = TL.Parse<InputFile>(reader);
            crop = TL.Parse<InputPhotoCrop>(reader);
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


        public override Constructor Constructor => Constructor.inputChatPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb2e1bf08);
            id.Write(writer);
            crop.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = TL.Parse<InputPhoto>(reader);
            crop = TL.Parse<InputPhotoCrop>(reader);
        }

        public override string ToString()
        {
            return $"(inputChatPhoto id:{id} crop:{crop})";
        }
    }


    public class InputGeoPointEmptyConstructor : InputGeoPoint
    {

        public InputGeoPointEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputGeoPointEmpty;

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


        public override Constructor Constructor => Constructor.inputGeoPoint;

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

        public InputPhotoEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputPhotoEmpty;

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
        public long access_hash;

        public InputPhotoConstructor()
        {

        }

        public InputPhotoConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfb95c6c4);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputPhoto id:{id} access_hash:{access_hash})";
        }
    }


    public class InputVideoEmptyConstructor : InputVideo
    {

        public InputVideoEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputVideoEmpty;

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
        public long access_hash;

        public InputVideoConstructor()
        {

        }

        public InputVideoConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xee579652);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputVideo id:{id} access_hash:{access_hash})";
        }
    }


    public class InputFileLocationConstructor : InputFileLocation
    {
        public long volume_id;
        public int local_id;
        public long secret;

        public InputFileLocationConstructor()
        {

        }

        public InputFileLocationConstructor(long volume_id, int local_id, long secret)
        {
            this.volume_id = volume_id;
            this.local_id = local_id;
            this.secret = secret;
        }


        public override Constructor Constructor => Constructor.inputFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x14637196);
            writer.Write(volume_id);
            writer.Write(local_id);
            writer.Write(secret);
        }

        public override void Read(BinaryReader reader)
        {
            volume_id = reader.ReadInt64();
            local_id = reader.ReadInt32();
            secret = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputFileLocation volume_id:{volume_id} local_id:{local_id} secret:{secret})";
        }
    }


    public class InputVideoFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long access_hash;

        public InputVideoFileLocationConstructor()
        {

        }

        public InputVideoFileLocationConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputVideoFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3d0364ec);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputVideoFileLocation id:{id} access_hash:{access_hash})";
        }
    }


    public class InputPhotoCropAutoConstructor : InputPhotoCrop
    {

        public InputPhotoCropAutoConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputPhotoCropAuto;

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
        public double crop_left;
        public double crop_top;
        public double crop_width;

        public InputPhotoCropConstructor()
        {

        }

        public InputPhotoCropConstructor(double crop_left, double crop_top, double crop_width)
        {
            this.crop_left = crop_left;
            this.crop_top = crop_top;
            this.crop_width = crop_width;
        }


        public override Constructor Constructor => Constructor.inputPhotoCrop;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd9915325);
            writer.Write(crop_left);
            writer.Write(crop_top);
            writer.Write(crop_width);
        }

        public override void Read(BinaryReader reader)
        {
            crop_left = reader.ReadDouble();
            crop_top = reader.ReadDouble();
            crop_width = reader.ReadDouble();
        }

        public override string ToString()
        {
            return $"(inputPhotoCrop crop_left:{crop_left} crop_top:{crop_top} crop_width:{crop_width})";
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


        public override Constructor Constructor => Constructor.inputAppEvent;

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
        public int user_id;

        public PeerUserConstructor()
        {

        }

        public PeerUserConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.peerUser;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9db1bc6d);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(peerUser user_id:{user_id})";
        }
    }


    public class PeerChatConstructor : Peer
    {
        public int chat_id;

        public PeerChatConstructor()
        {

        }

        public PeerChatConstructor(int chat_id)
        {
            this.chat_id = chat_id;
        }


        public override Constructor Constructor => Constructor.peerChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xbad0e5bb);
            writer.Write(chat_id);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(peerChat chat_id:{chat_id})";
        }
    }


    public class Storage_fileUnknownConstructor : storage_FileType
    {

        public Storage_fileUnknownConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileUnknown;

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


    public class Storage_fileJpegConstructor : storage_FileType
    {

        public Storage_fileJpegConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileJpeg;

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


    public class Storage_fileGifConstructor : storage_FileType
    {

        public Storage_fileGifConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileGif;

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


    public class Storage_filePngConstructor : storage_FileType
    {

        public Storage_filePngConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_filePng;

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


    public class Storage_fileMp3Constructor : storage_FileType
    {

        public Storage_fileMp3Constructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileMp3;

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


    public class Storage_fileMovConstructor : storage_FileType
    {

        public Storage_fileMovConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileMov;

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


    public class Storage_filePartialConstructor : storage_FileType
    {

        public Storage_filePartialConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_filePartial;

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


    public class Storage_fileMp4Constructor : storage_FileType
    {

        public Storage_fileMp4Constructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileMp4;

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


    public class Storage_fileWebpConstructor : storage_FileType
    {

        public Storage_fileWebpConstructor()
        {

        }



        public override Constructor Constructor => Constructor.storage_fileWebp;

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
        public long volume_id;
        public int local_id;
        public long secret;

        public FileLocationUnavailableConstructor()
        {

        }

        public FileLocationUnavailableConstructor(long volume_id, int local_id, long secret)
        {
            this.volume_id = volume_id;
            this.local_id = local_id;
            this.secret = secret;
        }


        public override Constructor Constructor => Constructor.fileLocationUnavailable;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7c596b46);
            writer.Write(volume_id);
            writer.Write(local_id);
            writer.Write(secret);
        }

        public override void Read(BinaryReader reader)
        {
            volume_id = reader.ReadInt64();
            local_id = reader.ReadInt32();
            secret = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(fileLocationUnavailable volume_id:{volume_id} local_id:{local_id} secret:{secret})";
        }
    }


    public class FileLocationConstructor : FileLocation
    {
        public int dc_id;
        public long volume_id;
        public int local_id;
        public long secret;

        public FileLocationConstructor()
        {

        }

        public FileLocationConstructor(int dc_id, long volume_id, int local_id, long secret)
        {
            this.dc_id = dc_id;
            this.volume_id = volume_id;
            this.local_id = local_id;
            this.secret = secret;
        }


        public override Constructor Constructor => Constructor.fileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x53d69076);
            writer.Write(dc_id);
            writer.Write(volume_id);
            writer.Write(local_id);
            writer.Write(secret);
        }

        public override void Read(BinaryReader reader)
        {
            dc_id = reader.ReadInt32();
            volume_id = reader.ReadInt64();
            local_id = reader.ReadInt32();
            secret = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(fileLocation dc_id:{dc_id} volume_id:{volume_id} local_id:{local_id} secret:{secret})";
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


        public override Constructor Constructor => Constructor.userEmpty;

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
        public string first_name;
        public string last_name;
        public string username;
        public string phone;
        public UserProfilePhoto photo;
        public UserStatus status;
        public bool inactive;

        public UserSelfConstructor()
        {

        }

        public UserSelfConstructor(int id, string first_name, string last_name, string username, string phone, UserProfilePhoto photo,
            UserStatus status, bool inactive)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.phone = phone;
            this.photo = photo;
            this.status = status;
            this.inactive = inactive;
        }


        public override Constructor Constructor => Constructor.userSelf;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x720535ec);
            writer.Write(id);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
            Serializers.String.Write(writer, username);
            Serializers.String.Write(writer, phone);
            photo.Write(writer);
            status.Write(writer);
            writer.Write(inactive ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            phone = Serializers.String.Read(reader);
            photo = TL.Parse<UserProfilePhoto>(reader);
            status = TL.Parse<UserStatus>(reader);
            inactive = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return
                $"(userSelf id:{id} first_name:'{first_name}' last_name:'{last_name}' username: '{username}' phone:'{phone}' photo:{photo} status:{status} inactive:{inactive})";
        }
    }


    public class UserContactConstructor : User //userContact#cab35e18 id:int first_name:string last_name:string username:string access_hash:long phone:string photo:UserProfilePhoto status:UserStatus = User;
    {
        public int id;
        public string firstName;
        public string lastName;
        public string username;
        public long access_hash;
        public string phone;
        public UserProfilePhoto photo;
        public UserStatus status;

        public UserContactConstructor()
        {

        }

        public UserContactConstructor(int id, string firstName, string lastName, string username, long access_hash, string phone,
            UserProfilePhoto photo, UserStatus status)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.access_hash = access_hash;
            this.phone = phone;
            this.photo = photo;
            this.status = status;
        }


        public override Constructor Constructor => Constructor.userContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xcab35e18);
            writer.Write(id);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
            Serializers.String.Write(writer, username);
            writer.Write(access_hash);
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
            access_hash = reader.ReadInt64();
            phone = Serializers.String.Read(reader);
            photo = TL.Parse<UserProfilePhoto>(reader);
            status = TL.Parse<UserStatus>(reader);
        }

        public override string ToString()
        {
            return
                $"(userContact id:{id} first_name:'{firstName}' last_name:'{lastName}' username: '{username}' access_hash:{access_hash} phone:'{phone}' photo:{photo} status:{status})";
        }
    }


    public class UserRequestConstructor : User //userRequest#d9ccc4ef id:int first_name:string last_name:string username:string access_hash:long phone:string photo:UserProfilePhoto status:UserStatus = User;
    {
        public int id;
        public string first_name;
        public string last_name;
        public string username;
        public long access_hash;
        public string phone;
        public UserProfilePhoto photo;
        public UserStatus status;

        public UserRequestConstructor()
        {

        }

        public UserRequestConstructor(int id, string first_name, string last_name, string username, long access_hash, string phone,
            UserProfilePhoto photo, UserStatus status)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.access_hash = access_hash;
            this.phone = phone;
            this.photo = photo;
            this.status = status;
        }


        public override Constructor Constructor => Constructor.userRequest;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x22e8ceb0);
            writer.Write(id);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
            Serializers.String.Write(writer, username);
            writer.Write(access_hash);
            Serializers.String.Write(writer, phone);
            photo.Write(writer);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            access_hash = reader.ReadInt64();
            phone = Serializers.String.Read(reader);
            photo = TL.Parse<UserProfilePhoto>(reader);
            status = TL.Parse<UserStatus>(reader);
        }

        public override string ToString()
        {
            return
                $"(userRequest id:{id} first_name:'{first_name}' last_name:'{last_name}' username:'{username}' access_hash:{access_hash} phone:'{phone}' photo:{photo} status:{status})";
        }
    }


    public class UserForeignConstructor : User //userForeign#75cf7a8 id:int first_name:string last_name:string username:string access_hash:long photo:UserProfilePhoto status:UserStatus = User;
    {
        public int id;
        public string first_name;
        public string last_name;
        public string username;
        public long access_hash;
        public UserProfilePhoto photo;
        public UserStatus status;

        public UserForeignConstructor()
        {

        }

        public UserForeignConstructor(int id, string first_name, string last_name, string username, long access_hash, UserProfilePhoto photo, UserStatus status)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.access_hash = access_hash;
            this.photo = photo;
            this.status = status;
        }


        public override Constructor Constructor => Constructor.userForeign;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5214c89d);
            writer.Write(id);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
            Serializers.String.Write(writer, username);
            writer.Write(access_hash);
            photo.Write(writer);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
            access_hash = reader.ReadInt64();
            photo = TL.Parse<UserProfilePhoto>(reader);
            status = TL.Parse<UserStatus>(reader);
            long tamano = reader.BaseStream.Length;
        }

        public override string ToString()
        {
            return
                $"(userForeign id:{id} first_name:'{first_name}' last_name:'{last_name}' username:'{username}' access_hash:{access_hash} photo:{photo} status:{status})";
        }
    }


    public class UserDeletedConstructor : User //userDeleted#d6016d7a id:int first_name:string last_name:string username:string = User;
    {
        public int id;
        public string first_name;
        public string last_name;
        public string username;

        public UserDeletedConstructor()
        {

        }

        public UserDeletedConstructor(int id, string first_name, string last_name, string username)
        {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
        }


        public override Constructor Constructor => Constructor.userDeleted;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb29ad7cc);
            writer.Write(id);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
            Serializers.String.Write(writer, username);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
            username = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(userDeleted id:{id} first_name:'{first_name}' last_name:'{last_name}' username: '{username}')";
        }
    }


    public class UserProfilePhotoEmptyConstructor : UserProfilePhoto
    {

        public UserProfilePhotoEmptyConstructor()
        {

        }

        public override Constructor Constructor => Constructor.userProfilePhotoEmpty;

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
        public long photo_id;
        public FileLocation photo_small;
        public FileLocation photo_big;

        public UserProfilePhotoConstructor()
        {

        }

        public UserProfilePhotoConstructor(long photo_id, FileLocation photo_small, FileLocation photo_big)
        {
            this.photo_id = photo_id;
            this.photo_small = photo_small;
            this.photo_big = photo_big;
        }


        public override Constructor Constructor => Constructor.userProfilePhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd559d8c8);
            writer.Write(photo_id);
            photo_small.Write(writer);
            photo_big.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photo_id = reader.ReadInt64();
            photo_small = TL.Parse<FileLocation>(reader);
            photo_big = TL.Parse<FileLocation>(reader);
        }

        public override string ToString()
        {
            return $"(userProfilePhoto photo_id:{photo_id} photo_small:{photo_small} photo_big:{photo_big})";
        }
    }


    public class UserStatusEmptyConstructor : UserStatus
    {

        public UserStatusEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.userStatusEmpty;

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


        public override Constructor Constructor => Constructor.userStatusOnline;

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
        public int was_online;

        public UserStatusOfflineConstructor()
        {

        }

        public UserStatusOfflineConstructor(int was_online)
        {
            this.was_online = was_online;
        }


        public override Constructor Constructor => Constructor.userStatusOffline;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x008c703f);
            writer.Write(was_online);
        }

        public override void Read(BinaryReader reader)
        {
            was_online = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(userStatusOffline was_online:{was_online})";
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


        public override Constructor Constructor => Constructor.chatEmpty;

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
        public int participants_count;
        public int date;
        public bool left;
        public int version;

        public ChatConstructor()
        {

        }

        public ChatConstructor(int id, string title, ChatPhoto photo, int participants_count, int date, bool left, int version)
        {
            this.id = id;
            this.title = title;
            this.photo = photo;
            this.participants_count = participants_count;
            this.date = date;
            this.left = left;
            this.version = version;
        }


        public override Constructor Constructor => Constructor.chat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6e9c9bc7);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            photo.Write(writer);
            writer.Write(participants_count);
            writer.Write(date);
            writer.Write(left ? 0x997275b5 : 0xbc799737);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            photo = TL.Parse<ChatPhoto>(reader);
            participants_count = reader.ReadInt32();
            date = reader.ReadInt32();
            left = reader.ReadUInt32() == 0x997275b5;
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(chat id:{id} title:'{title}' photo:{photo} participants_count:{participants_count} date:{date} left:{left} version:{version})";
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


        public override Constructor Constructor => Constructor.chatForbidden;

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
        public Photo chat_photo;
        public PeerNotifySettings notify_settings;

        public ChatFullConstructor()
        {

        }

        public ChatFullConstructor(int id, ChatParticipants participants, Photo chat_photo, PeerNotifySettings notify_settings)
        {
            this.id = id;
            this.participants = participants;
            this.chat_photo = chat_photo;
            this.notify_settings = notify_settings;
        }


        public override Constructor Constructor => Constructor.chatFull;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x630e61be);
            writer.Write(id);
            participants.Write(writer);
            chat_photo.Write(writer);
            notify_settings.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            participants = TL.Parse<ChatParticipants>(reader);
            chat_photo = TL.Parse<Photo>(reader);
            notify_settings = TL.Parse<PeerNotifySettings>(reader);
        }

        public override string ToString()
        {
            return
                $"(chatFull id:{id} participants:{participants} chat_photo:{chat_photo} notify_settings:{notify_settings})";
        }
    }


    public class ChatParticipantConstructor : ChatParticipant
    {
        public int user_id;
        public int inviter_id;
        public int date;

        public ChatParticipantConstructor()
        {

        }

        public ChatParticipantConstructor(int user_id, int inviter_id, int date)
        {
            this.user_id = user_id;
            this.inviter_id = inviter_id;
            this.date = date;
        }


        public override Constructor Constructor => Constructor.chatParticipant;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc8d7493e);
            writer.Write(user_id);
            writer.Write(inviter_id);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            inviter_id = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatParticipant user_id:{user_id} inviter_id:{inviter_id} date:{date})";
        }
    }


    public class ChatParticipantsForbiddenConstructor : ChatParticipants
    {
        public int chat_id;

        public ChatParticipantsForbiddenConstructor()
        {

        }

        public ChatParticipantsForbiddenConstructor(int chat_id)
        {
            this.chat_id = chat_id;
        }


        public override Constructor Constructor => Constructor.chatParticipantsForbidden;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0fd2bb8a);
            writer.Write(chat_id);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatParticipantsForbidden chat_id:{chat_id})";
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


        public override Constructor Constructor => Constructor.chatParticipants;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7841b415);
            writer.Write(chatId);
            writer.Write(adminId);
            writer.Write(0x1cb5c415);
            writer.Write(participants.Count);
            foreach (ChatParticipant participants_element in participants)
            {
                participants_element.Write(writer);
            }
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            chatId = reader.ReadInt32();
            adminId = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int participants_len = reader.ReadInt32();
            participants = new List<ChatParticipant>(participants_len);
            for (int participants_index = 0; participants_index < participants_len; participants_index++)
            {
                ChatParticipant participants_element;
                participants_element = TL.Parse<ChatParticipant>(reader);
                participants.Add(participants_element);
            }
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(chatParticipants chat_id:{chatId} admin_id:{adminId} participants:{Serializers.VectorToString(participants)} version:{version})";
        }
    }


    public class ChatPhotoEmptyConstructor : ChatPhoto
    {

        public ChatPhotoEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.chatPhotoEmpty;

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
        public FileLocation photo_small;
        public FileLocation photo_big;

        public ChatPhotoConstructor()
        {

        }

        public ChatPhotoConstructor(FileLocation photo_small, FileLocation photo_big)
        {
            this.photo_small = photo_small;
            this.photo_big = photo_big;
        }


        public override Constructor Constructor => Constructor.chatPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6153276a);
            photo_small.Write(writer);
            photo_big.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photo_small = TL.Parse<FileLocation>(reader);
            photo_big = TL.Parse<FileLocation>(reader);
        }

        public override string ToString()
        {
            return $"(chatPhoto photo_small:{photo_small} photo_big:{photo_big})";
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


        public override Constructor Constructor => Constructor.messageEmpty;

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

        public bool Unread => (flags & 0x1) > 0;
        public bool SentByCurrentUser => (flags & 0x2) > 0;

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

        public override Constructor Constructor => Constructor.message;

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
            toId = TL.Parse<Peer>(reader);
            date = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            media = TL.Parse<MessageMedia>(reader);
        }

        public override string ToString()
        {
            return $"(message) Flags: {flags}, Id: {id}, FromId: {fromId}, ToId: {toId}, Date: {date}, Message: {message}, Media: {media}, Unread: {Unread}, SentByCurrentUser: {SentByCurrentUser}, Constructor: {Constructor}";
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

        public bool Unread => (flags & 0x1) > 0;
        public bool SentByCurrentUser => (flags & 0x2) > 0;

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

        public override Constructor Constructor => Constructor.messageForwarded;

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
            toId = TL.Parse<Peer>(reader);
            date = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            media = TL.Parse<MessageMedia>(reader);
        }

        public override string ToString()
        {
            return $"(messageForwarded) Flags: {flags}, Id: {id}, FwdFromId: {fwdFromId}, FwdDate: {fwdDate}, FromId: {fromId}, ToId: {toId}, Date: {date}, Message: {message}, Media: {media}, Unread: {Unread}, SentByCurrentUser: {SentByCurrentUser}, Constructor: {Constructor}";
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

        public bool Unread => (flags & 0x1) > 0;
        public bool SentByCurrentUser => (flags & 0x2) > 0;

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

        public override Constructor Constructor => Constructor.messageService;

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
            toId = TL.Parse<Peer>(reader);
            date = reader.ReadInt32();
            action = TL.Parse<MessageAction>(reader);
        }

        public override string ToString()
        {
            return $"(messageService) Flags: {flags}, Id: {id}, FromId: {fromId}, ToId: {toId}, Date: {date}, Action: {action}, Unread: {Unread}, SentByCurrentUser: {SentByCurrentUser}, Constructor: {Constructor}";
        }
    }


    public class MessageMediaEmptyConstructor : MessageMedia
    {

        public MessageMediaEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.messageMediaEmpty;

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


        public override Constructor Constructor => Constructor.messageMediaPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc8c45a2a);
            photo.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photo = TL.Parse<Photo>(reader);
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


        public override Constructor Constructor => Constructor.messageMediaVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa2d24290);
            video.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            video = TL.Parse<Video>(reader);
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


        public override Constructor Constructor => Constructor.messageMediaGeo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x56e0d474);
            geo.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            geo = TL.Parse<GeoPoint>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaGeo geo:{geo})";
        }
    }


    public class MessageMediaContactConstructor : MessageMedia
    {
        public string phone_number;
        public string first_name;
        public string last_name;
        public int user_id;

        public MessageMediaContactConstructor()
        {

        }

        public MessageMediaContactConstructor(string phone_number, string first_name, string last_name, int user_id)
        {
            this.phone_number = phone_number;
            this.first_name = first_name;
            this.last_name = last_name;
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.messageMediaContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5e7d2f39);
            Serializers.String.Write(writer, phone_number);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            phone_number = Serializers.String.Read(reader);
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(messageMediaContact phone_number:'{phone_number}' first_name:'{first_name}' last_name:'{last_name}' user_id:{user_id})";
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


        public override Constructor Constructor => Constructor.messageMediaUnsupported;

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

        public MessageActionEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.messageActionEmpty;

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


        public override Constructor Constructor => Constructor.messageActionChatCreate;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa6638b9a);
            Serializers.String.Write(writer, title);
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (int users_element in users)
            {
                writer.Write(users_element);
            }
        }

        public override void Read(BinaryReader reader)
        {
            title = Serializers.String.Read(reader);
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<int>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                int users_element;
                users_element = reader.ReadInt32();
                users.Add(users_element);
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


        public override Constructor Constructor => Constructor.messageActionChatEditTitle;

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


        public override Constructor Constructor => Constructor.messageActionChatEditPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x7fcb13a8);
            photo.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            photo = TL.Parse<Photo>(reader);
        }

        public override string ToString()
        {
            return $"(messageActionChatEditPhoto photo:{photo})";
        }
    }


    public class MessageActionChatDeletePhotoConstructor : MessageAction
    {

        public MessageActionChatDeletePhotoConstructor()
        {

        }



        public override Constructor Constructor => Constructor.messageActionChatDeletePhoto;

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
        public int user_id;

        public MessageActionChatAddUserConstructor()
        {

        }

        public MessageActionChatAddUserConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.messageActionChatAddUser;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5e3cfc4b);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messageActionChatAddUser user_id:{user_id})";
        }
    }


    public class MessageActionChatDeleteUserConstructor : MessageAction
    {
        public int user_id;

        public MessageActionChatDeleteUserConstructor()
        {

        }

        public MessageActionChatDeleteUserConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.messageActionChatDeleteUser;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb2ae9b0c);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(messageActionChatDeleteUser user_id:{user_id})";
        }
    }

    public class ContactsContacts
    {
        public IList<Contact> Contacts { get; set; }
        public IList<User> Users { get; set; }
    }

    public class MessageDialogs
    {
        public int? Count { get; set; }
        public List<Dialog> Dialogs { get; set; }
        public List<Message> Messages { get; set; }
        public List<Chat> Chats { get; set; }
        public List<User> Users { get; set; }
    }

    public class DialogConstructor : Dialog
    {
        public Peer peer;
        public int top_message;
        public int unread_count;
        public PeerNotifySettings peerNotifySettings;

        public DialogConstructor()
        {

        }

        public DialogConstructor(Peer peer, int top_message, int unread_count, PeerNotifySettings peerNotifySettings)
        {
            this.peer = peer;
            this.top_message = top_message;
            this.unread_count = unread_count;
            this.peerNotifySettings = peerNotifySettings;
        }


        public override Constructor Constructor => Constructor.dialog;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xab3a99ac);
            peer.Write(writer);
            writer.Write(top_message);
            writer.Write(unread_count);
            peerNotifySettings.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            peer = TL.Parse<Peer>(reader);
            top_message = reader.ReadInt32();
            unread_count = reader.ReadInt32();
            peerNotifySettings = TL.Parse<PeerNotifySettings>(reader);
        }

        public override string ToString()
        {
            return $"(dialog peer:{peer} top_message:{top_message} unread_count:{unread_count})";
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


        public override Constructor Constructor => Constructor.photoEmpty;

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
        public long access_hash;
        public int user_id;
        public int date;
        public string caption;
        public GeoPoint geo;
        public List<PhotoSize> sizes;

        public PhotoConstructor()
        {

        }

        public PhotoConstructor(long id, long access_hash, int user_id, int date, string caption, GeoPoint geo,
            List<PhotoSize> sizes)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.user_id = user_id;
            this.date = date;
            this.caption = caption;
            this.geo = geo;
            this.sizes = sizes;
        }


        public override Constructor Constructor => Constructor.photo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x22b56751);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(user_id);
            writer.Write(date);
            Serializers.String.Write(writer, caption);
            geo.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(sizes.Count);
            foreach (PhotoSize sizes_element in sizes)
            {
                sizes_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
            user_id = reader.ReadInt32();
            date = reader.ReadInt32();
            caption = Serializers.String.Read(reader);
            geo = TL.Parse<GeoPoint>(reader);
            reader.ReadInt32(); // vector code
            int sizes_len = reader.ReadInt32();
            sizes = new List<PhotoSize>(sizes_len);
            for (int sizes_index = 0; sizes_index < sizes_len; sizes_index++)
            {
                PhotoSize sizes_element;
                sizes_element = TL.Parse<PhotoSize>(reader);
                sizes.Add(sizes_element);
            }
        }

        public override string ToString()
        {
            return
                $"(photo id:{id} access_hash:{access_hash} user_id:{user_id} date:{date} caption:'{caption}' geo:{geo} sizes:{Serializers.VectorToString(sizes)})";
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


        public override Constructor Constructor => Constructor.photoSizeEmpty;

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


        public override Constructor Constructor => Constructor.photoSize;

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
            location = TL.Parse<FileLocation>(reader);
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


        public override Constructor Constructor => Constructor.photoCachedSize;

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
            location = TL.Parse<FileLocation>(reader);
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


        public override Constructor Constructor => Constructor.videoEmpty;

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
        public long access_hash;
        public int user_id;
        public int date;
        public string caption;
        public int duration;
        public int size;
        public PhotoSize thumb;
        public int dc_id;
        public int w;
        public int h;

        public VideoConstructor()
        {

        }

        public VideoConstructor(long id, long access_hash, int user_id, int date, string caption, int duration, int size,
            PhotoSize thumb, int dc_id, int w, int h)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.user_id = user_id;
            this.date = date;
            this.caption = caption;
            this.duration = duration;
            this.size = size;
            this.thumb = thumb;
            this.dc_id = dc_id;
            this.w = w;
            this.h = h;
        }


        public override Constructor Constructor => Constructor.video;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5a04a49f);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(user_id);
            writer.Write(date);
            Serializers.String.Write(writer, caption);
            writer.Write(duration);
            writer.Write(size);
            thumb.Write(writer);
            writer.Write(dc_id);
            writer.Write(w);
            writer.Write(h);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
            user_id = reader.ReadInt32();
            date = reader.ReadInt32();
            caption = Serializers.String.Read(reader);
            duration = reader.ReadInt32();
            size = reader.ReadInt32();
            thumb = TL.Parse<PhotoSize>(reader);
            dc_id = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(video id:{id} access_hash:{access_hash} user_id:{user_id} date:{date} caption:'{caption}' duration:{duration} size:{size} thumb:{thumb} dc_id:{dc_id} w:{w} h:{h})";
        }
    }


    public class GeoPointEmptyConstructor : GeoPoint
    {

        public GeoPointEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.geoPointEmpty;

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


        public override Constructor Constructor => Constructor.geoPoint;

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


    public class Auth_checkedPhoneConstructor : auth_CheckedPhone
    {
        public bool phone_registered;
        public bool phone_invited;

        public Auth_checkedPhoneConstructor()
        {

        }

        public Auth_checkedPhoneConstructor(bool phone_registered, bool phone_invited)
        {
            this.phone_registered = phone_registered;
            this.phone_invited = phone_invited;
        }


        public override Constructor Constructor => Constructor.auth_checkedPhone;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe300cc3b);
            writer.Write(phone_registered ? 0x997275b5 : 0xbc799737);
            writer.Write(phone_invited ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            phone_registered = reader.ReadUInt32() == 0x997275b5;
            phone_invited = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(auth_checkedPhone phone_registered:{phone_registered} phone_invited:{phone_invited})";
        }
    }


    public class AuthSentCodeConstructor : AuthSentCode
    {
        public override Constructor Constructor => Constructor.authSentCode;
    }

    public class AuthSentAppCodeConstructor : AuthSentCode
    {
        public override Constructor Constructor => Constructor.authSentAppCode;
    }

    public class Auth_authorizationConstructor : auth_Authorization
    {
        public int expires;
        public User user;

        public Auth_authorizationConstructor()
        {

        }

        public Auth_authorizationConstructor(int expires, User user)
        {
            this.expires = expires;
            this.user = user;
        }


        public override Constructor Constructor => Constructor.auth_authorization;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf6b673a4);
            writer.Write(expires);
            user.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            expires = reader.ReadInt32();
            user = TL.Parse<User>(reader);
        }

        public override string ToString()
        {
            return $"(auth_authorization expires:{expires} user:{user})";
        }
    }


    public class Auth_exportedAuthorizationConstructor : auth_ExportedAuthorization
    {
        public int id;
        public byte[] bytes;

        public Auth_exportedAuthorizationConstructor()
        {

        }

        public Auth_exportedAuthorizationConstructor(int id, byte[] bytes)
        {
            this.id = id;
            this.bytes = bytes;
        }


        public override Constructor Constructor => Constructor.auth_exportedAuthorization;

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


        public override Constructor Constructor => Constructor.inputNotifyPeer;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb8bc5b0c);
            peer.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            peer = TL.Parse<InputPeer>(reader);
        }

        public override string ToString()
        {
            return $"(inputNotifyPeer peer:{peer})";
        }
    }


    public class InputNotifyUsersConstructor : InputNotifyPeer
    {

        public InputNotifyUsersConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputNotifyUsers;

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

        public InputNotifyChatsConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputNotifyChats;

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

        public InputNotifyAllConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputNotifyAll;

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

        public InputPeerNotifyEventsEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputPeerNotifyEventsEmpty;

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

        public InputPeerNotifyEventsAllConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputPeerNotifyEventsAll;

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
        public int mute_until;
        public string sound;
        public bool show_previews;
        public int events_mask;

        public InputPeerNotifySettingsConstructor()
        {

        }

        public InputPeerNotifySettingsConstructor(int mute_until, string sound, bool show_previews, int events_mask)
        {
            this.mute_until = mute_until;
            this.sound = sound;
            this.show_previews = show_previews;
            this.events_mask = events_mask;
        }


        public override Constructor Constructor => Constructor.inputPeerNotifySettings;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x46a2ce98);
            writer.Write(mute_until);
            Serializers.String.Write(writer, sound);
            writer.Write(show_previews ? 0x997275b5 : 0xbc799737);
            writer.Write(events_mask);
        }

        public override void Read(BinaryReader reader)
        {
            mute_until = reader.ReadInt32();
            sound = Serializers.String.Read(reader);
            show_previews = reader.ReadUInt32() == 0x997275b5;
            events_mask = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(inputPeerNotifySettings mute_until:{mute_until} sound:'{sound}' show_previews:{show_previews} events_mask:{events_mask})";
        }
    }


    public class PeerNotifyEventsEmptyConstructor : PeerNotifyEvents
    {

        public PeerNotifyEventsEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.peerNotifyEventsEmpty;

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

        public PeerNotifyEventsAllConstructor()
        {

        }



        public override Constructor Constructor => Constructor.peerNotifyEventsAll;

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

        public PeerNotifySettingsEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.peerNotifySettingsEmpty;

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
        public int mute_until;
        public string sound;
        public bool show_previews;
        public int events_mask;

        public PeerNotifySettingsConstructor()
        {

        }

        public PeerNotifySettingsConstructor(int mute_until, string sound, bool show_previews, int events_mask)
        {
            this.mute_until = mute_until;
            this.sound = sound;
            this.show_previews = show_previews;
            this.events_mask = events_mask;
        }


        public override Constructor Constructor => Constructor.peerNotifySettings;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8d5e11ee);
            writer.Write(mute_until);
            Serializers.String.Write(writer, sound);
            writer.Write(show_previews ? 0x997275b5 : 0xbc799737);
            writer.Write(events_mask);
        }

        public override void Read(BinaryReader reader)
        {
            mute_until = reader.ReadInt32();
            sound = Serializers.String.Read(reader);
            show_previews = reader.ReadUInt32() == 0x997275b5;
            events_mask = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(peerNotifySettings mute_until:{mute_until} sound:'{sound}' show_previews:{show_previews} events_mask:{events_mask})";
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


        public override Constructor Constructor => Constructor.wallPaper;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xccb03657);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            writer.Write(0x1cb5c415);
            writer.Write(sizes.Count);
            foreach (PhotoSize sizes_element in sizes)
            {
                sizes_element.Write(writer);
            }
            writer.Write(color);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            reader.ReadInt32(); // vector code
            int sizes_len = reader.ReadInt32();
            sizes = new List<PhotoSize>(sizes_len);
            for (int sizes_index = 0; sizes_index < sizes_len; sizes_index++)
            {
                PhotoSize sizes_element;
                sizes_element = TL.Parse<PhotoSize>(reader);
                sizes.Add(sizes_element);
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
        public contacts_Link link;
        public Photo profile_photo;
        public PeerNotifySettings notify_settings;
        public bool blocked;
        public string real_first_name;
        public string real_last_name;

        public UserFullConstructor()
        {

        }

        public UserFullConstructor(User user, contacts_Link link, Photo profile_photo, PeerNotifySettings notify_settings,
            bool blocked, string real_first_name, string real_last_name)
        {
            this.user = user;
            this.link = link;
            this.profile_photo = profile_photo;
            this.notify_settings = notify_settings;
            this.blocked = blocked;
            this.real_first_name = real_first_name;
            this.real_last_name = real_last_name;
        }


        public override Constructor Constructor => Constructor.userFull;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x771095da);
            user.Write(writer);
            link.Write(writer);
            profile_photo.Write(writer);
            notify_settings.Write(writer);
            writer.Write(blocked ? 0x997275b5 : 0xbc799737);
            Serializers.String.Write(writer, real_first_name);
            Serializers.String.Write(writer, real_last_name);
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
            link = TL.Parse<contacts_Link>(reader);
            profile_photo = TL.Parse<Photo>(reader);
            notify_settings = TL.Parse<PeerNotifySettings>(reader);
            blocked = reader.ReadUInt32() == 0x997275b5;
            real_first_name = Serializers.String.Read(reader);
            real_last_name = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(userFull user:{user} link:{link} profile_photo:{profile_photo} notify_settings:{notify_settings} blocked:{blocked} real_first_name:'{real_first_name}' real_last_name:'{real_last_name}')";
        }
    }


    public class ContactConstructor : Contact
    {
        public int user_id;
        public bool mutual;

        public ContactConstructor()
        {

        }

        public ContactConstructor(int user_id, bool mutual)
        {
            this.user_id = user_id;
            this.mutual = mutual;
        }


        public override Constructor Constructor => Constructor.contact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf911c994);
            writer.Write(user_id);
            writer.Write(mutual ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            mutual = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(contact user_id:{user_id} mutual:{mutual})";
        }
    }


    public class ImportedContactConstructor : ImportedContact
    {
        public int user_id;
        public long client_id;

        public ImportedContactConstructor()
        {

        }

        public ImportedContactConstructor(int user_id, long client_id)
        {
            this.user_id = user_id;
            this.client_id = client_id;
        }


        public override Constructor Constructor => Constructor.importedContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd0028438);
            writer.Write(user_id);
            writer.Write(client_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            client_id = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(importedContact user_id:{user_id} client_id:{client_id})";
        }
    }


    public class ContactBlockedConstructor : ContactBlocked
    {
        public int user_id;
        public int date;

        public ContactBlockedConstructor()
        {

        }

        public ContactBlockedConstructor(int user_id, int date)
        {
            this.user_id = user_id;
            this.date = date;
        }


        public override Constructor Constructor => Constructor.contactBlocked;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x561bc879);
            writer.Write(user_id);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactBlocked user_id:{user_id} date:{date})";
        }
    }


    public class ContactFoundConstructor : ContactFound
    {
        public int user_id;

        public ContactFoundConstructor()
        {

        }

        public ContactFoundConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.contactFound;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xea879f95);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactFound user_id:{user_id})";
        }
    }


    public class ContactSuggestedConstructor : ContactSuggested
    {
        public int user_id;
        public int mutual_contacts;

        public ContactSuggestedConstructor()
        {

        }

        public ContactSuggestedConstructor(int user_id, int mutual_contacts)
        {
            this.user_id = user_id;
            this.mutual_contacts = mutual_contacts;
        }


        public override Constructor Constructor => Constructor.contactSuggested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3de191a1);
            writer.Write(user_id);
            writer.Write(mutual_contacts);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            mutual_contacts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactSuggested user_id:{user_id} mutual_contacts:{mutual_contacts})";
        }
    }


    public class ContactStatusConstructor : ContactStatus
    {
        public int user_id;
        public int expires;

        public ContactStatusConstructor()
        {

        }

        public ContactStatusConstructor(int user_id, int expires)
        {
            this.user_id = user_id;
            this.expires = expires;
        }


        public override Constructor Constructor => Constructor.contactStatus;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xaa77b873);
            writer.Write(user_id);
            writer.Write(expires);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            expires = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(contactStatus user_id:{user_id} expires:{expires})";
        }
    }


    public class ChatLocatedConstructor : ChatLocated
    {
        public int chat_id;
        public int distance;

        public ChatLocatedConstructor()
        {

        }

        public ChatLocatedConstructor(int chat_id, int distance)
        {
            this.chat_id = chat_id;
            this.distance = distance;
        }


        public override Constructor Constructor => Constructor.chatLocated;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3631cf4c);
            writer.Write(chat_id);
            writer.Write(distance);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            distance = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(chatLocated chat_id:{chat_id} distance:{distance})";
        }
    }


    public class Contacts_foreignLinkUnknownConstructor : contacts_ForeignLink
    {

        public Contacts_foreignLinkUnknownConstructor()
        {

        }



        public override Constructor Constructor => Constructor.contacts_foreignLinkUnknown;

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


    public class Contacts_foreignLinkRequestedConstructor : contacts_ForeignLink
    {
        public bool has_phone;

        public Contacts_foreignLinkRequestedConstructor()
        {

        }

        public Contacts_foreignLinkRequestedConstructor(bool has_phone)
        {
            this.has_phone = has_phone;
        }


        public override Constructor Constructor => Constructor.contacts_foreignLinkRequested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa7801f47);
            writer.Write(has_phone ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            has_phone = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(contacts_foreignLinkRequested has_phone:{has_phone})";
        }
    }


    public class Contacts_foreignLinkMutualConstructor : contacts_ForeignLink
    {

        public Contacts_foreignLinkMutualConstructor()
        {

        }



        public override Constructor Constructor => Constructor.contacts_foreignLinkMutual;

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


    public class Contacts_myLinkEmptyConstructor : contacts_MyLink
    {

        public Contacts_myLinkEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.contacts_myLinkEmpty;

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


    public class Contacts_myLinkRequestedConstructor : contacts_MyLink
    {
        public bool contact;

        public Contacts_myLinkRequestedConstructor()
        {

        }

        public Contacts_myLinkRequestedConstructor(bool contact)
        {
            this.contact = contact;
        }


        public override Constructor Constructor => Constructor.contacts_myLinkRequested;

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


    public class Contacts_myLinkContactConstructor : contacts_MyLink
    {

        public Contacts_myLinkContactConstructor()
        {

        }



        public override Constructor Constructor => Constructor.contacts_myLinkContact;

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


    public class Contacts_linkConstructor : contacts_Link
    {
        public contacts_MyLink my_link;
        public contacts_ForeignLink foreign_link;
        public User user;

        public Contacts_linkConstructor()
        {

        }

        public Contacts_linkConstructor(contacts_MyLink my_link, contacts_ForeignLink foreign_link, User user)
        {
            this.my_link = my_link;
            this.foreign_link = foreign_link;
            this.user = user;
        }


        public override Constructor Constructor => Constructor.contacts_link;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xeccea3f5);
            my_link.Write(writer);
            foreign_link.Write(writer);
            user.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            my_link = TL.Parse<contacts_MyLink>(reader);
            foreign_link = TL.Parse<contacts_ForeignLink>(reader);
            user = TL.Parse<User>(reader);
        }

        public override string ToString()
        {
            return $"(contacts_link my_link:{my_link} foreign_link:{foreign_link} user:{user})";
        }
    }


    public class Contacts_contactsConstructor : contacts_Contacts
    {
        public List<Contact> contacts;
        public List<User> users;

        public Contacts_contactsConstructor()
        {

        }

        public Contacts_contactsConstructor(List<Contact> contacts, List<User> users)
        {
            this.contacts = contacts;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.contacts_contacts;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6f8b8cb2);
            writer.Write(0x1cb5c415);
            writer.Write(contacts.Count);
            foreach (Contact contacts_element in contacts)
            {
                contacts_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int contacts_len = reader.ReadInt32();
            contacts = new List<Contact>(contacts_len);
            for (int contacts_index = 0; contacts_index < contacts_len; contacts_index++)
            {
                Contact contacts_element;
                contacts_element = TL.Parse<Contact>(reader);
                contacts.Add(contacts_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_contacts contacts:{Serializers.VectorToString(contacts)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Contacts_contactsNotModifiedConstructor : contacts_Contacts
    {

        public Contacts_contactsNotModifiedConstructor()
        {

        }



        public override Constructor Constructor => Constructor.contacts_contactsNotModified;

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


    public class ContactsImportedContactsConstructor : contacts_ImportedContacts
    {
        public List<ImportedContact> importedContacts;
        public List<long> retryContacts;
        public List<User> users;

        public override Constructor Constructor => Constructor.contacts_importedContacts;

        public override void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void Read(BinaryReader reader)
        {
            importedContacts = TL.ParseVector<ImportedContact>(reader);
            retryContacts = TL.ParseVector(reader, reader.ReadInt64);
            users = TL.ParseVector<User>(reader);
        }

        public override string ToString()
        {
            return $"({Constructor}) ImportedContacts: {importedContacts}, RetryContacts: {retryContacts}, Users: {users}";
        }
    }


    public class Contacts_blockedConstructor : contacts_Blocked
    {
        public List<ContactBlocked> blocked;
        public List<User> users;

        public Contacts_blockedConstructor()
        {

        }

        public Contacts_blockedConstructor(List<ContactBlocked> blocked, List<User> users)
        {
            this.blocked = blocked;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.contacts_blocked;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1c138d15);
            writer.Write(0x1cb5c415);
            writer.Write(blocked.Count);
            foreach (ContactBlocked blocked_element in blocked)
            {
                blocked_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int blocked_len = reader.ReadInt32();
            blocked = new List<ContactBlocked>(blocked_len);
            for (int blocked_index = 0; blocked_index < blocked_len; blocked_index++)
            {
                ContactBlocked blocked_element;
                blocked_element = TL.Parse<ContactBlocked>(reader);
                blocked.Add(blocked_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_blocked blocked:{Serializers.VectorToString(blocked)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Contacts_blockedSliceConstructor : contacts_Blocked
    {
        public int count;
        public List<ContactBlocked> blocked;
        public List<User> users;

        public Contacts_blockedSliceConstructor()
        {

        }

        public Contacts_blockedSliceConstructor(int count, List<ContactBlocked> blocked, List<User> users)
        {
            this.count = count;
            this.blocked = blocked;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.contacts_blockedSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x900802a1);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(blocked.Count);
            foreach (ContactBlocked blocked_element in blocked)
            {
                blocked_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int blocked_len = reader.ReadInt32();
            blocked = new List<ContactBlocked>(blocked_len);
            for (int blocked_index = 0; blocked_index < blocked_len; blocked_index++)
            {
                ContactBlocked blocked_element;
                blocked_element = TL.Parse<ContactBlocked>(reader);
                blocked.Add(blocked_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_blockedSlice count:{count} blocked:{Serializers.VectorToString(blocked)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Contacts_foundConstructor : contacts_Found
    {
        public List<ContactFound> results;
        public List<User> users;

        public Contacts_foundConstructor()
        {

        }

        public Contacts_foundConstructor(List<ContactFound> results, List<User> users)
        {
            this.results = results;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.contacts_found;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0566000e);
            writer.Write(0x1cb5c415);
            writer.Write(results.Count);
            foreach (ContactFound results_element in results)
            {
                results_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int results_len = reader.ReadInt32();
            results = new List<ContactFound>(results_len);
            for (int results_index = 0; results_index < results_len; results_index++)
            {
                ContactFound results_element;
                results_element = TL.Parse<ContactFound>(reader);
                results.Add(results_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_found results:{Serializers.VectorToString(results)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Contacts_suggestedConstructor : contacts_Suggested
    {
        public List<ContactSuggested> results;
        public List<User> users;

        public Contacts_suggestedConstructor()
        {

        }

        public Contacts_suggestedConstructor(List<ContactSuggested> results, List<User> users)
        {
            this.results = results;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.contacts_suggested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5649dcc5);
            writer.Write(0x1cb5c415);
            writer.Write(results.Count);
            foreach (ContactSuggested results_element in results)
            {
                results_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int results_len = reader.ReadInt32();
            results = new List<ContactSuggested>(results_len);
            for (int results_index = 0; results_index < results_len; results_index++)
            {
                ContactSuggested results_element;
                results_element = TL.Parse<ContactSuggested>(reader);
                results.Add(results_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(contacts_suggested results:{Serializers.VectorToString(results)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_dialogsConstructor : messages_Dialogs
    {
        public List<Dialog> dialogs;
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public Messages_dialogsConstructor()
        {

        }

        public Messages_dialogsConstructor(List<Dialog> dialogs, List<Message> messages, List<Chat> chats, List<User> users)
        {
            this.dialogs = dialogs;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_dialogs;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x15ba6c40);
            writer.Write(0x1cb5c415);
            writer.Write(dialogs.Count);
            foreach (Dialog dialogs_element in dialogs)
            {
                dialogs_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int dialogs_len = reader.ReadInt32();
            dialogs = new List<Dialog>(dialogs_len);
            for (int dialogs_index = 0; dialogs_index < dialogs_len; dialogs_index++)
            {
                Dialog dialogs_element;
                dialogs_element = TL.Parse<Dialog>(reader);
                dialogs.Add(dialogs_element);
            }
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<Message>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                Message messages_element;
                messages_element = TL.Parse<Message>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_dialogs dialogs:{Serializers.VectorToString(dialogs)} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_dialogsSliceConstructor : messages_Dialogs
    {
        public int count;
        public List<Dialog> dialogs;
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public Messages_dialogsSliceConstructor()
        {

        }

        public Messages_dialogsSliceConstructor(int count, List<Dialog> dialogs, List<Message> messages, List<Chat> chats,
            List<User> users)
        {
            this.count = count;
            this.dialogs = dialogs;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_dialogsSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x71e094f3);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(dialogs.Count);
            foreach (Dialog dialogs_element in dialogs)
            {
                dialogs_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int dialogs_len = reader.ReadInt32();
            dialogs = new List<Dialog>(dialogs_len);
            for (int dialogs_index = 0; dialogs_index < dialogs_len; dialogs_index++)
            {
                Dialog dialogs_element;
                dialogs_element = TL.Parse<Dialog>(reader);
                dialogs.Add(dialogs_element);
            }
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<Message>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                Message messages_element;
                messages_element = TL.Parse<Message>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_dialogsSlice count:{count} dialogs:{Serializers.VectorToString(dialogs)} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_messagesConstructor : messages_Messages
    {
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public Messages_messagesConstructor()
        {

        }

        public Messages_messagesConstructor(List<Message> messages, List<Chat> chats, List<User> users)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_messages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8c718e87);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<Message>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                Message messages_element;
                messages_element = TL.Parse<Message>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_messages messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_messagesSliceConstructor : messages_Messages
    {
        public int count;
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;

        public Messages_messagesSliceConstructor()
        {

        }

        public Messages_messagesSliceConstructor(int count, List<Message> messages, List<Chat> chats, List<User> users)
        {
            this.count = count;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_messagesSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x0b446ae3);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<Message>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                Message messages_element;
                messages_element = TL.Parse<Message>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_messagesSlice count:{count} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_messageEmptyConstructor : messages_Message
    {

        public Messages_messageEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.messages_messageEmpty;

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


    public class Messages_messageConstructor : messages_Message
    {
        public Message message;
        public List<Chat> chats;
        public List<User> users;

        public Messages_messageConstructor()
        {

        }

        public Messages_messageConstructor(Message message, List<Chat> chats, List<User> users)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_message;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xff90c417);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<Message>(reader);
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_message message:{message} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_statedMessagesConstructor : messages_StatedMessages
    {
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;
        public int pts;
        public int seq;

        public Messages_statedMessagesConstructor()
        {

        }

        public Messages_statedMessagesConstructor(List<Message> messages, List<Chat> chats, List<User> users, int pts, int seq)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.messages_statedMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x969478bb);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<Message>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                Message messages_element;
                messages_element = TL.Parse<Message>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
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


    public class Messages_statedMessageConstructor : messages_StatedMessage
    {
        public Message message;
        public List<Chat> chats;
        public List<User> users;
        public int pts;
        public int seq;

        public Messages_statedMessageConstructor() { }

        public Messages_statedMessageConstructor(Message message, List<Chat> chats, List<User> users, int pts, int seq)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
            this.pts = pts;
            this.seq = seq;
        }

        public override Constructor Constructor => Constructor.messages_statedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd07ae726);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<Message>(reader);
            chats = TL.ParseVector<Chat>(reader);
            users = TL.ParseVector<User>(reader);

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


        public override Constructor Constructor => Constructor.messages_sentMessage;

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


    public class Messages_chatConstructor : messages_Chat
    {
        public Chat chat;
        public List<User> users;

        public Messages_chatConstructor()
        {

        }

        public Messages_chatConstructor(Chat chat, List<User> users)
        {
            this.chat = chat;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_chat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x40e9002a);
            chat.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            chat = TL.Parse<Chat>(reader);
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return $"(messages_chat chat:{chat} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_chatsConstructor : messages_Chats
    {
        public List<Chat> chats;
        public List<User> users;

        public Messages_chatsConstructor()
        {

        }

        public Messages_chatsConstructor(List<Chat> chats, List<User> users)
        {
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_chats;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8150cbd8);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_chats chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_chatFullConstructor : messages_ChatFull
    {
        public ChatFull full_chat;
        public List<Chat> chats;
        public List<User> users;

        public Messages_chatFullConstructor()
        {

        }

        public Messages_chatFullConstructor(ChatFull full_chat, List<Chat> chats, List<User> users)
        {
            this.full_chat = full_chat;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.messages_chatFull;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe5d7d19c);
            full_chat.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            full_chat = TL.Parse<ChatFull>(reader);
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(messages_chatFull full_chat:{full_chat} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Messages_affectedHistoryConstructor : messages_AffectedHistory
    {
        public int pts;
        public int seq;
        public int offset;

        public Messages_affectedHistoryConstructor()
        {

        }

        public Messages_affectedHistoryConstructor(int pts, int seq, int offset)
        {
            this.pts = pts;
            this.seq = seq;
            this.offset = offset;
        }


        public override Constructor Constructor => Constructor.messages_affectedHistory;

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

        public InputMessagesFilterEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputMessagesFilterEmpty;

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

        public InputMessagesFilterPhotosConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputMessagesFilterPhotos;

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

        public InputMessagesFilterVideoConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputMessagesFilterVideo;

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

        public InputMessagesFilterPhotoVideoConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputMessagesFilterPhotoVideo;

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


        public override Constructor Constructor => Constructor.updateNewMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x013abdb3);
            message.Write(writer);
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<Message>(reader);
            pts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateNewMessage message:{message} pts:{pts})";
        }
    }


    public class UpdateMessageIDConstructor : Update
    {
        public int id;
        public long random_id;

        public UpdateMessageIDConstructor()
        {

        }

        public UpdateMessageIDConstructor(int id, long random_id)
        {
            this.id = id;
            this.random_id = random_id;
        }


        public override Constructor Constructor => Constructor.updateMessageID;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4e90bfd6);
            writer.Write(id);
            writer.Write(random_id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            random_id = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(updateMessageID id:{id} random_id:{random_id})";
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


        public override Constructor Constructor => Constructor.updateReadMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc6649e31);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (int messages_element in messages)
            {
                writer.Write(messages_element);
            }
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<int>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                int messages_element;
                messages_element = reader.ReadInt32();
                messages.Add(messages_element);
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


        public override Constructor Constructor => Constructor.updateDeleteMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa92bfe26);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (int messages_element in messages)
            {
                writer.Write(messages_element);
            }
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<int>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                int messages_element;
                messages_element = reader.ReadInt32();
                messages.Add(messages_element);
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


        public override Constructor Constructor => Constructor.updateRestoreMessages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd15de04d);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (int messages_element in messages)
            {
                writer.Write(messages_element);
            }
            writer.Write(pts);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<int>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                int messages_element;
                messages_element = reader.ReadInt32();
                messages.Add(messages_element);
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
        public int user_id;

        public UpdateUserTypingConstructor()
        {

        }

        public UpdateUserTypingConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.updateUserTyping;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5c486927);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateUserTyping user_id:{user_id})";
        }
    }


    public class UpdateChatUserTypingConstructor : Update
    {
        public int chat_id;
        public int user_id;

        public UpdateChatUserTypingConstructor()
        {

        }

        public UpdateChatUserTypingConstructor(int chat_id, int user_id)
        {
            this.chat_id = chat_id;
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.updateChatUserTyping;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9a65ea1f);
            writer.Write(chat_id);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateChatUserTyping chat_id:{chat_id} user_id:{user_id})";
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


        public override Constructor Constructor => Constructor.updateChatParticipants;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x07761198);
            participants.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            participants = TL.Parse<ChatParticipants>(reader);
        }

        public override string ToString()
        {
            return $"(updateChatParticipants participants:{participants})";
        }
    }


    public class UpdateUserStatusConstructor : Update
    {
        public int user_id;
        public UserStatus status;

        public UpdateUserStatusConstructor()
        {

        }

        public UpdateUserStatusConstructor(int user_id, UserStatus status)
        {
            this.user_id = user_id;
            this.status = status;
        }


        public override Constructor Constructor => Constructor.updateUserStatus;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1bfbd823);
            writer.Write(user_id);
            status.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            status = TL.Parse<UserStatus>(reader);
        }

        public override string ToString()
        {
            return $"(updateUserStatus user_id:{user_id} status:{status})";
        }
    }


    public class UpdateUserNameConstructor : Update
    {
        public int user_id;
        public string first_name;
        public string last_name;

        public UpdateUserNameConstructor()
        {

        }

        public UpdateUserNameConstructor(int user_id, string first_name, string last_name)
        {
            this.user_id = user_id;
            this.first_name = first_name;
            this.last_name = last_name;
        }


        public override Constructor Constructor => Constructor.updateUserName;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa7332b73);
            writer.Write(user_id);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(updateUserName user_id:{user_id} first_name:'{first_name}' last_name:'{last_name}')";
        }
    }


    public class UpdateUserPhotoConstructor : Update
    {
        public int user_id;
        public int date;
        public UserProfilePhoto photo;
        public bool previous;

        public UpdateUserPhotoConstructor()
        {

        }

        public UpdateUserPhotoConstructor(int user_id, int date, UserProfilePhoto photo, bool previous)
        {
            this.user_id = user_id;
            this.date = date;
            this.photo = photo;
            this.previous = previous;
        }


        public override Constructor Constructor => Constructor.updateUserPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x95313b0c);
            writer.Write(user_id);
            writer.Write(date);
            photo.Write(writer);
            writer.Write(previous ? 0x997275b5 : 0xbc799737);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            date = reader.ReadInt32();
            photo = TL.Parse<UserProfilePhoto>(reader);
            previous = reader.ReadUInt32() == 0x997275b5;
        }

        public override string ToString()
        {
            return $"(updateUserPhoto user_id:{user_id} date:{date} photo:{photo} previous:{previous})";
        }
    }


    public class UpdateContactRegisteredConstructor : Update
    {
        public int user_id;
        public int date;

        public UpdateContactRegisteredConstructor()
        {

        }

        public UpdateContactRegisteredConstructor(int user_id, int date)
        {
            this.user_id = user_id;
            this.date = date;
        }


        public override Constructor Constructor => Constructor.updateContactRegistered;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2575bbb9);
            writer.Write(user_id);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateContactRegistered user_id:{user_id} date:{date})";
        }
    }


    public class UpdateContactLinkConstructor : Update
    {
        public int user_id;
        public contacts_MyLink my_link;
        public contacts_ForeignLink foreign_link;

        public UpdateContactLinkConstructor()
        {

        }

        public UpdateContactLinkConstructor(int user_id, contacts_MyLink my_link, contacts_ForeignLink foreign_link)
        {
            this.user_id = user_id;
            this.my_link = my_link;
            this.foreign_link = foreign_link;
        }


        public override Constructor Constructor => Constructor.updateContactLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x51a48a9a);
            writer.Write(user_id);
            my_link.Write(writer);
            foreign_link.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
            my_link = TL.Parse<contacts_MyLink>(reader);
            foreign_link = TL.Parse<contacts_ForeignLink>(reader);
        }

        public override string ToString()
        {
            return $"(updateContactLink user_id:{user_id} my_link:{my_link} foreign_link:{foreign_link})";
        }
    }


    public class UpdateActivationConstructor : Update
    {
        public int user_id;

        public UpdateActivationConstructor()
        {

        }

        public UpdateActivationConstructor(int user_id)
        {
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.updateActivation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6f690963);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateActivation user_id:{user_id})";
        }
    }


    public class UpdateNewAuthorizationConstructor : Update
    {
        public long auth_key_id;
        public int date;
        public string device;
        public string location;

        public UpdateNewAuthorizationConstructor()
        {

        }

        public UpdateNewAuthorizationConstructor(long auth_key_id, int date, string device, string location)
        {
            this.auth_key_id = auth_key_id;
            this.date = date;
            this.device = device;
            this.location = location;
        }


        public override Constructor Constructor => Constructor.updateNewAuthorization;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8f06529a);
            writer.Write(auth_key_id);
            writer.Write(date);
            Serializers.String.Write(writer, device);
            Serializers.String.Write(writer, location);
        }

        public override void Read(BinaryReader reader)
        {
            auth_key_id = reader.ReadInt64();
            date = reader.ReadInt32();
            device = Serializers.String.Read(reader);
            location = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(updateNewAuthorization auth_key_id:{auth_key_id} date:{date} device:'{device}' location:'{location}')";
        }
    }


    public class Updates_stateConstructor : updates_State
    {
        public int pts;
        public int qts;
        public int date;
        public int seq;
        public int unread_count;

        public Updates_stateConstructor()
        {

        }

        public Updates_stateConstructor(int pts, int qts, int date, int seq, int unread_count)
        {
            this.pts = pts;
            this.qts = qts;
            this.date = date;
            this.seq = seq;
            this.unread_count = unread_count;
        }


        public override Constructor Constructor => Constructor.updates_state;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa56c2a3e);
            writer.Write(pts);
            writer.Write(qts);
            writer.Write(date);
            writer.Write(seq);
            writer.Write(unread_count);
        }

        public override void Read(BinaryReader reader)
        {
            pts = reader.ReadInt32();
            qts = reader.ReadInt32();
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
            unread_count = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updates_state pts:{pts} qts:{qts} date:{date} seq:{seq} unread_count:{unread_count})";
        }
    }


    public class Updates_differenceEmptyConstructor : UpdatesDifference
    {
        public int date;
        public int seq;

        public Updates_differenceEmptyConstructor()
        {

        }

        public Updates_differenceEmptyConstructor(int date, int seq)
        {
            this.date = date;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.updates_differenceEmpty;

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


    public class Updates_differenceConstructor : UpdatesDifference
    {
        public List<Message> new_messages;
        public List<EncryptedMessage> new_encrypted_messages;
        public List<Update> other_updates;
        public List<Chat> chats;
        public List<User> users;
        public updates_State state;

        public Updates_differenceConstructor()
        {

        }

        public Updates_differenceConstructor(List<Message> new_messages, List<EncryptedMessage> new_encrypted_messages,
            List<Update> other_updates, List<Chat> chats, List<User> users, updates_State state)
        {
            this.new_messages = new_messages;
            this.new_encrypted_messages = new_encrypted_messages;
            this.other_updates = other_updates;
            this.chats = chats;
            this.users = users;
            this.state = state;
        }


        public override Constructor Constructor => Constructor.updates_difference;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x00f49ca0);
            writer.Write(0x1cb5c415);
            writer.Write(new_messages.Count);
            foreach (Message new_messages_element in new_messages)
            {
                new_messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(new_encrypted_messages.Count);
            foreach (EncryptedMessage new_encrypted_messages_element in new_encrypted_messages)
            {
                new_encrypted_messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(other_updates.Count);
            foreach (Update other_updates_element in other_updates)
            {
                other_updates_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            state.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int new_messages_len = reader.ReadInt32();
            new_messages = new List<Message>(new_messages_len);
            for (int new_messages_index = 0; new_messages_index < new_messages_len; new_messages_index++)
            {
                Message new_messages_element;
                new_messages_element = TL.Parse<Message>(reader);
                new_messages.Add(new_messages_element);
            }
            reader.ReadInt32(); // vector code
            int new_encrypted_messages_len = reader.ReadInt32();
            new_encrypted_messages = new List<EncryptedMessage>(new_encrypted_messages_len);
            for (int new_encrypted_messages_index = 0;
                new_encrypted_messages_index < new_encrypted_messages_len;
                new_encrypted_messages_index++)
            {
                EncryptedMessage new_encrypted_messages_element;
                new_encrypted_messages_element = TL.Parse<EncryptedMessage>(reader);
                new_encrypted_messages.Add(new_encrypted_messages_element);
            }
            reader.ReadInt32(); // vector code
            int other_updates_len = reader.ReadInt32();
            other_updates = new List<Update>(other_updates_len);
            for (int other_updates_index = 0; other_updates_index < other_updates_len; other_updates_index++)
            {
                Update other_updates_element;
                other_updates_element = TL.Parse<Update>(reader);
                other_updates.Add(other_updates_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            state = TL.Parse<updates_State>(reader);
        }

        public override string ToString()
        {
            return
                $"(updates_difference new_messages:{Serializers.VectorToString(new_messages)} new_encrypted_messages:{Serializers.VectorToString(new_encrypted_messages)} other_updates:{Serializers.VectorToString(other_updates)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} state:{state})";
        }
    }


    public class Updates_differenceSliceConstructor : UpdatesDifference
    {
        public List<Message> new_messages;
        public List<EncryptedMessage> new_encrypted_messages;
        public List<Update> other_updates;
        public List<Chat> chats;
        public List<User> users;
        public updates_State intermediate_state;

        public Updates_differenceSliceConstructor()
        {

        }

        public Updates_differenceSliceConstructor(List<Message> new_messages, List<EncryptedMessage> new_encrypted_messages,
            List<Update> other_updates, List<Chat> chats, List<User> users, updates_State intermediate_state)
        {
            this.new_messages = new_messages;
            this.new_encrypted_messages = new_encrypted_messages;
            this.other_updates = other_updates;
            this.chats = chats;
            this.users = users;
            this.intermediate_state = intermediate_state;
        }


        public override Constructor Constructor => Constructor.updates_differenceSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa8fb1981);
            writer.Write(0x1cb5c415);
            writer.Write(new_messages.Count);
            foreach (Message new_messages_element in new_messages)
            {
                new_messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(new_encrypted_messages.Count);
            foreach (EncryptedMessage new_encrypted_messages_element in new_encrypted_messages)
            {
                new_encrypted_messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(other_updates.Count);
            foreach (Update other_updates_element in other_updates)
            {
                other_updates_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            intermediate_state.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int new_messages_len = reader.ReadInt32();
            new_messages = new List<Message>(new_messages_len);
            for (int new_messages_index = 0; new_messages_index < new_messages_len; new_messages_index++)
            {
                Message new_messages_element;
                new_messages_element = TL.Parse<Message>(reader);
                new_messages.Add(new_messages_element);
            }
            reader.ReadInt32(); // vector code
            int new_encrypted_messages_len = reader.ReadInt32();
            new_encrypted_messages = new List<EncryptedMessage>(new_encrypted_messages_len);
            for (int new_encrypted_messages_index = 0;
                new_encrypted_messages_index < new_encrypted_messages_len;
                new_encrypted_messages_index++)
            {
                EncryptedMessage new_encrypted_messages_element;
                new_encrypted_messages_element = TL.Parse<EncryptedMessage>(reader);
                new_encrypted_messages.Add(new_encrypted_messages_element);
            }
            reader.ReadInt32(); // vector code
            int other_updates_len = reader.ReadInt32();
            other_updates = new List<Update>(other_updates_len);
            for (int other_updates_index = 0; other_updates_index < other_updates_len; other_updates_index++)
            {
                Update other_updates_element;
                other_updates_element = TL.Parse<Update>(reader);
                other_updates.Add(other_updates_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            intermediate_state = TL.Parse<updates_State>(reader);
        }

        public override string ToString()
        {
            return
                $"(updates_differenceSlice new_messages:{Serializers.VectorToString(new_messages)} new_encrypted_messages:{Serializers.VectorToString(new_encrypted_messages)} other_updates:{Serializers.VectorToString(other_updates)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} intermediate_state:{intermediate_state})";
        }
    }


    public class UpdatesTooLongConstructor : Updates
    {

        public UpdatesTooLongConstructor()
        {

        }



        public override Constructor Constructor => Constructor.updatesTooLong;

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
        public int from_id;
        public string message;
        public int pts;
        public int date;
        public int seq;

        public UpdateShortMessageConstructor()
        {

        }

        public UpdateShortMessageConstructor(int id, int from_id, string message, int pts, int date, int seq)
        {
            this.id = id;
            this.from_id = from_id;
            this.message = message;
            this.pts = pts;
            this.date = date;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.updateShortMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd3f45784);
            writer.Write(id);
            writer.Write(from_id);
            Serializers.String.Write(writer, message);
            writer.Write(pts);
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            from_id = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            pts = reader.ReadInt32();
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateShortMessage id:{id} from_id:{from_id} message:'{message}' pts:{pts} date:{date} seq:{seq})";
        }
    }


    public class UpdateShortChatMessageConstructor : Updates
    {
        public int id;
        public int from_id;
        public int chat_id;
        public string message;
        public int pts;
        public int date;
        public int seq;

        public UpdateShortChatMessageConstructor()
        {

        }

        public UpdateShortChatMessageConstructor(int id, int from_id, int chat_id, string message, int pts, int date, int seq)
        {
            this.id = id;
            this.from_id = from_id;
            this.chat_id = chat_id;
            this.message = message;
            this.pts = pts;
            this.date = date;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.updateShortChatMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2b2fbd4e);
            writer.Write(id);
            writer.Write(from_id);
            writer.Write(chat_id);
            Serializers.String.Write(writer, message);
            writer.Write(pts);
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            from_id = reader.ReadInt32();
            chat_id = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            pts = reader.ReadInt32();
            date = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updateShortChatMessage id:{id} from_id:{from_id} chat_id:{chat_id} message:'{message}' pts:{pts} date:{date} seq:{seq})";
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


        public override Constructor Constructor => Constructor.updateShort;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x78d4dec1);
            update.Write(writer);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            update = TL.Parse<Update>(reader);
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
        public int seq_start;
        public int seq;

        public UpdatesCombinedConstructor()
        {

        }

        public UpdatesCombinedConstructor(List<Update> updates, List<User> users, List<Chat> chats, int date, int seq_start,
            int seq)
        {
            this.updates = updates;
            this.users = users;
            this.chats = chats;
            this.date = date;
            this.seq_start = seq_start;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.updatesCombined;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x725b04c3);
            writer.Write(0x1cb5c415);
            writer.Write(updates.Count);
            foreach (Update updates_element in updates)
            {
                updates_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(date);
            writer.Write(seq_start);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int updates_len = reader.ReadInt32();
            updates = new List<Update>(updates_len);
            for (int updates_index = 0; updates_index < updates_len; updates_index++)
            {
                Update updates_element;
                updates_element = TL.Parse<Update>(reader);
                updates.Add(updates_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            date = reader.ReadInt32();
            seq_start = reader.ReadInt32();
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updatesCombined updates:{Serializers.VectorToString(updates)} users:{Serializers.VectorToString(users)} chats:{Serializers.VectorToString(chats)} date:{date} seq_start:{seq_start} seq:{seq})";
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


        public override Constructor Constructor => Constructor.updates;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x74ae4240);
            writer.Write(0x1cb5c415);
            writer.Write(updates.Count);
            foreach (Update updates_element in updates)
            {
                updates_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(date);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int updates_len = reader.ReadInt32();
            updates = new List<Update>(updates_len);
            for (int updates_index = 0; updates_index < updates_len; updates_index++)
            {
                Update updates_element;
                updates_element = TL.Parse<Update>(reader);
                updates.Add(updates_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
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


    public class Photos_photosConstructor : photos_Photos
    {
        public List<Photo> photos;
        public List<User> users;

        public Photos_photosConstructor()
        {

        }

        public Photos_photosConstructor(List<Photo> photos, List<User> users)
        {
            this.photos = photos;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.photos_photos;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8dca6aa5);
            writer.Write(0x1cb5c415);
            writer.Write(photos.Count);
            foreach (Photo photos_element in photos)
            {
                photos_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int photos_len = reader.ReadInt32();
            photos = new List<Photo>(photos_len);
            for (int photos_index = 0; photos_index < photos_len; photos_index++)
            {
                Photo photos_element;
                photos_element = TL.Parse<Photo>(reader);
                photos.Add(photos_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(photos_photos photos:{Serializers.VectorToString(photos)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Photos_photosSliceConstructor : photos_Photos
    {
        public int count;
        public List<Photo> photos;
        public List<User> users;

        public Photos_photosSliceConstructor()
        {

        }

        public Photos_photosSliceConstructor(int count, List<Photo> photos, List<User> users)
        {
            this.count = count;
            this.photos = photos;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.photos_photosSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x15051f54);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(photos.Count);
            foreach (Photo photos_element in photos)
            {
                photos_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int photos_len = reader.ReadInt32();
            photos = new List<Photo>(photos_len);
            for (int photos_index = 0; photos_index < photos_len; photos_index++)
            {
                Photo photos_element;
                photos_element = TL.Parse<Photo>(reader);
                photos.Add(photos_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(photos_photosSlice count:{count} photos:{Serializers.VectorToString(photos)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Photos_photoConstructor : photos_Photo
    {
        public Photo photo;
        public List<User> users;

        public Photos_photoConstructor()
        {

        }

        public Photos_photoConstructor(Photo photo, List<User> users)
        {
            this.photo = photo;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.photos_photo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x20212ca8);
            photo.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            photo = TL.Parse<Photo>(reader);
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return $"(photos_photo photo:{photo} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Upload_fileConstructor : upload_File
    {
        public storage_FileType type;
        public int mtime;
        public byte[] bytes;

        public Upload_fileConstructor()
        {

        }

        public Upload_fileConstructor(storage_FileType type, int mtime, byte[] bytes)
        {
            this.type = type;
            this.mtime = mtime;
            this.bytes = bytes;
        }


        public override Constructor Constructor => Constructor.upload_file;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x096a18d5);
            type.Write(writer);
            writer.Write(mtime);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            type = TL.Parse<storage_FileType>(reader);
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
        public string ip_address;
        public int port;

        public DcOptionConstructor()
        {

        }

        public DcOptionConstructor(int id, string hostname, string ip_address, int port)
        {
            this.id = id;
            this.hostname = hostname;
            this.ip_address = ip_address;
            this.port = port;
        }


        public override Constructor Constructor => Constructor.dcOption;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2ec2a43c);
            writer.Write(id);
            Serializers.String.Write(writer, hostname);
            Serializers.String.Write(writer, ip_address);
            writer.Write(port);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            hostname = Serializers.String.Read(reader);
            ip_address = Serializers.String.Read(reader);
            port = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(dcOption id:{id} hostname:'{hostname}' ip_address:'{ip_address}' port:{port})";
        }
    }


    public class ConfigConstructor : Config
    {
        public int date;
        public bool test_mode;
        public int this_dc;
        public List<DcOption> dc_options;
        public int chat_size_max;

        public ConfigConstructor()
        {

        }

        public ConfigConstructor(int date, bool test_mode, int this_dc, List<DcOption> dc_options, int chat_size_max)
        {
            this.date = date;
            this.test_mode = test_mode;
            this.this_dc = this_dc;
            this.dc_options = dc_options;
            this.chat_size_max = chat_size_max;
        }


        public override Constructor Constructor => Constructor.config;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x232d5905);
            writer.Write(date);
            writer.Write(test_mode ? 0x997275b5 : 0xbc799737);
            writer.Write(this_dc);
            writer.Write(0x1cb5c415);
            writer.Write(dc_options.Count);
            foreach (DcOption dc_options_element in dc_options)
            {
                dc_options_element.Write(writer);
            }
            writer.Write(chat_size_max);
        }

        public override void Read(BinaryReader reader)
        {
            date = reader.ReadInt32();
            var expires = reader.ReadInt32();
            test_mode = reader.ReadUInt32() == 0x997275b5;
            this_dc = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int dc_options_len = reader.ReadInt32();
            dc_options = new List<DcOption>(dc_options_len);
            for (int dc_options_index = 0; dc_options_index < dc_options_len; dc_options_index++)
            {
                DcOption dc_options_element;
                dc_options_element = TL.Parse<DcOption>(reader);
                dc_options.Add(dc_options_element);
            }
            chat_size_max = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(config date:{date} test_mode:{test_mode} this_dc:{this_dc} dc_options:{Serializers.VectorToString(dc_options)} chat_size_max:{chat_size_max})";
        }
    }


    public class NearestDcConstructor : NearestDc
    {
        public string country;
        public int this_dc;
        public int nearest_dc;

        public NearestDcConstructor()
        {

        }

        public NearestDcConstructor(string country, int this_dc, int nearest_dc)
        {
            this.country = country;
            this.this_dc = this_dc;
            this.nearest_dc = nearest_dc;
        }


        public override Constructor Constructor => Constructor.nearestDc;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8e1a1775);
            Serializers.String.Write(writer, country);
            writer.Write(this_dc);
            writer.Write(nearest_dc);
        }

        public override void Read(BinaryReader reader)
        {
            country = Serializers.String.Read(reader);
            this_dc = reader.ReadInt32();
            nearest_dc = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(nearestDc country:'{country}' this_dc:{this_dc} nearest_dc:{nearest_dc})";
        }
    }


    public class Help_appUpdateConstructor : help_AppUpdate
    {
        public int id;
        public bool critical;
        public string url;
        public string text;

        public Help_appUpdateConstructor()
        {

        }

        public Help_appUpdateConstructor(int id, bool critical, string url, string text)
        {
            this.id = id;
            this.critical = critical;
            this.url = url;
            this.text = text;
        }


        public override Constructor Constructor => Constructor.help_appUpdate;

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


    public class Help_noAppUpdateConstructor : help_AppUpdate
    {

        public Help_noAppUpdateConstructor()
        {

        }



        public override Constructor Constructor => Constructor.help_noAppUpdate;

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


    public class Help_inviteTextConstructor : help_InviteText
    {
        public string message;

        public Help_inviteTextConstructor()
        {

        }

        public Help_inviteTextConstructor(string message)
        {
            this.message = message;
        }


        public override Constructor Constructor => Constructor.help_inviteText;

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


    public class Messages_statedMessagesLinksConstructor : messages_StatedMessages
    {
        public List<Message> messages;
        public List<Chat> chats;
        public List<User> users;
        public List<contacts_Link> links;
        public int pts;
        public int seq;

        public Messages_statedMessagesLinksConstructor()
        {

        }

        public Messages_statedMessagesLinksConstructor(List<Message> messages, List<Chat> chats, List<User> users,
            List<contacts_Link> links, int pts, int seq)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
            this.links = links;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.messages_statedMessagesLinks;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3e74f5c6);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (Message messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(links.Count);
            foreach (contacts_Link links_element in links)
            {
                links_element.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<Message>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                Message messages_element;
                messages_element = TL.Parse<Message>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            reader.ReadInt32(); // vector code
            int links_len = reader.ReadInt32();
            links = new List<contacts_Link>(links_len);
            for (int links_index = 0; links_index < links_len; links_index++)
            {
                contacts_Link links_element;
                links_element = TL.Parse<contacts_Link>(reader);
                links.Add(links_element);
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


    public class Messages_statedMessageLinkConstructor : messages_StatedMessage
    {
        public Message message;
        public List<Chat> chats;
        public List<User> users;
        public List<contacts_Link> links;
        public int pts;
        public int seq;

        public Messages_statedMessageLinkConstructor()
        {

        }

        public Messages_statedMessageLinkConstructor(Message message, List<Chat> chats, List<User> users,
            List<contacts_Link> links, int pts, int seq)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
            this.links = links;
            this.pts = pts;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.messages_statedMessageLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa9af2881);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(links.Count);
            foreach (contacts_Link links_element in links)
            {
                links_element.Write(writer);
            }
            writer.Write(pts);
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<Message>(reader);
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            reader.ReadInt32(); // vector code
            int links_len = reader.ReadInt32();
            links = new List<contacts_Link>(links_len);
            for (int links_index = 0; links_index < links_len; links_index++)
            {
                contacts_Link links_element;
                links_element = TL.Parse<contacts_Link>(reader);
                links.Add(links_element);
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
        public List<contacts_Link> links;

        public SentMessageLinkConstructor()
        {

        }

        public SentMessageLinkConstructor(int id, int date, int pts, int seq, List<contacts_Link> links)
        {
            this.id = id;
            this.date = date;
            this.pts = pts;
            this.seq = seq;
            this.links = links;
        }


        public override Constructor Constructor => Constructor.messages_sentMessageLink;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xe9db4a3f);
            writer.Write(id);
            writer.Write(date);
            writer.Write(pts);
            writer.Write(seq);
            writer.Write(0x1cb5c415);
            writer.Write(links.Count);
            foreach (contacts_Link links_element in links)
            {
                links_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            date = reader.ReadInt32();
            pts = reader.ReadInt32();
            seq = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int links_len = reader.ReadInt32();
            links = new List<contacts_Link>(links_len);
            for (int links_index = 0; links_index < links_len; links_index++)
            {
                contacts_Link links_element;
                links_element = TL.Parse<contacts_Link>(reader);
                links.Add(links_element);
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
        public int chat_id;
        public long access_hash;

        public InputGeoChatConstructor()
        {

        }

        public InputGeoChatConstructor(int chat_id, long access_hash)
        {
            this.chat_id = chat_id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputGeoChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x74d456fa);
            writer.Write(chat_id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputGeoChat chat_id:{chat_id} access_hash:{access_hash})";
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


        public override Constructor Constructor => Constructor.inputNotifyGeoChatPeer;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4d8ddec8);
            peer.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            peer = TL.Parse<InputGeoChat>(reader);
        }

        public override string ToString()
        {
            return $"(inputNotifyGeoChatPeer peer:{peer})";
        }
    }


    public class GeoChatConstructor : Chat
    {
        public int id;
        public long access_hash;
        public string title;
        public string address;
        public string venue;
        public GeoPoint geo;
        public ChatPhoto photo;
        public int participants_count;
        public int date;
        public bool checked_in;
        public int version;

        public GeoChatConstructor()
        {

        }

        public GeoChatConstructor(int id, long access_hash, string title, string address, string venue, GeoPoint geo,
            ChatPhoto photo, int participants_count, int date, bool checked_in, int version)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.title = title;
            this.address = address;
            this.venue = venue;
            this.geo = geo;
            this.photo = photo;
            this.participants_count = participants_count;
            this.date = date;
            this.checked_in = checked_in;
            this.version = version;
        }


        public override Constructor Constructor => Constructor.geoChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x75eaea5a);
            writer.Write(id);
            writer.Write(access_hash);
            Serializers.String.Write(writer, title);
            Serializers.String.Write(writer, address);
            Serializers.String.Write(writer, venue);
            geo.Write(writer);
            photo.Write(writer);
            writer.Write(participants_count);
            writer.Write(date);
            writer.Write(checked_in ? 0x997275b5 : 0xbc799737);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
            title = Serializers.String.Read(reader);
            address = Serializers.String.Read(reader);
            venue = Serializers.String.Read(reader);
            geo = TL.Parse<GeoPoint>(reader);
            photo = TL.Parse<ChatPhoto>(reader);
            participants_count = reader.ReadInt32();
            date = reader.ReadInt32();
            checked_in = reader.ReadUInt32() == 0x997275b5;
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(geoChat id:{id} access_hash:{access_hash} title:'{title}' address:'{address}' venue:'{venue}' geo:{geo} photo:{photo} participants_count:{participants_count} date:{date} checked_in:{checked_in} version:{version})";
        }
    }


    public class GeoChatMessageEmptyConstructor : GeoChatMessage
    {
        public int chat_id;
        public int id;

        public GeoChatMessageEmptyConstructor()
        {

        }

        public GeoChatMessageEmptyConstructor(int chat_id, int id)
        {
            this.chat_id = chat_id;
            this.id = id;
        }


        public override Constructor Constructor => Constructor.geoChatMessageEmpty;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x60311a9b);
            writer.Write(chat_id);
            writer.Write(id);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(geoChatMessageEmpty chat_id:{chat_id} id:{id})";
        }
    }


    public class GeoChatMessageConstructor : GeoChatMessage
    {
        public int chat_id;
        public int id;
        public int from_id;
        public int date;
        public string message;
        public MessageMedia media;

        public GeoChatMessageConstructor()
        {

        }

        public GeoChatMessageConstructor(int chat_id, int id, int from_id, int date, string message, MessageMedia media)
        {
            this.chat_id = chat_id;
            this.id = id;
            this.from_id = from_id;
            this.date = date;
            this.message = message;
            this.media = media;
        }


        public override Constructor Constructor => Constructor.geoChatMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4505f8e1);
            writer.Write(chat_id);
            writer.Write(id);
            writer.Write(from_id);
            writer.Write(date);
            Serializers.String.Write(writer, message);
            media.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            id = reader.ReadInt32();
            from_id = reader.ReadInt32();
            date = reader.ReadInt32();
            message = Serializers.String.Read(reader);
            media = TL.Parse<MessageMedia>(reader);
        }

        public override string ToString()
        {
            return
                $"(geoChatMessage chat_id:{chat_id} id:{id} from_id:{from_id} date:{date} message:'{message}' media:{media})";
        }
    }


    public class GeoChatMessageServiceConstructor : GeoChatMessage
    {
        public int chat_id;
        public int id;
        public int from_id;
        public int date;
        public MessageAction action;

        public GeoChatMessageServiceConstructor()
        {

        }

        public GeoChatMessageServiceConstructor(int chat_id, int id, int from_id, int date, MessageAction action)
        {
            this.chat_id = chat_id;
            this.id = id;
            this.from_id = from_id;
            this.date = date;
            this.action = action;
        }


        public override Constructor Constructor => Constructor.geoChatMessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd34fa24e);
            writer.Write(chat_id);
            writer.Write(id);
            writer.Write(from_id);
            writer.Write(date);
            action.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            id = reader.ReadInt32();
            from_id = reader.ReadInt32();
            date = reader.ReadInt32();
            action = TL.Parse<MessageAction>(reader);
        }

        public override string ToString()
        {
            return $"(geoChatMessageService chat_id:{chat_id} id:{id} from_id:{from_id} date:{date} action:{action})";
        }
    }


    public class Geochats_statedMessageConstructor : geochats_StatedMessage
    {
        public GeoChatMessage message;
        public List<Chat> chats;
        public List<User> users;
        public int seq;

        public Geochats_statedMessageConstructor()
        {

        }

        public Geochats_statedMessageConstructor(GeoChatMessage message, List<Chat> chats, List<User> users, int seq)
        {
            this.message = message;
            this.chats = chats;
            this.users = users;
            this.seq = seq;
        }


        public override Constructor Constructor => Constructor.geochats_statedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x17b1578b);
            message.Write(writer);
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
            writer.Write(seq);
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<GeoChatMessage>(reader);
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
            seq = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(geochats_statedMessage message:{message} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)} seq:{seq})";
        }
    }


    public class Geochats_locatedConstructor : geochats_Located
    {
        public List<ChatLocated> results;
        public List<GeoChatMessage> messages;
        public List<Chat> chats;
        public List<User> users;

        public Geochats_locatedConstructor()
        {

        }

        public Geochats_locatedConstructor(List<ChatLocated> results, List<GeoChatMessage> messages, List<Chat> chats,
            List<User> users)
        {
            this.results = results;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.geochats_located;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x48feb267);
            writer.Write(0x1cb5c415);
            writer.Write(results.Count);
            foreach (ChatLocated results_element in results)
            {
                results_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (GeoChatMessage messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int results_len = reader.ReadInt32();
            results = new List<ChatLocated>(results_len);
            for (int results_index = 0; results_index < results_len; results_index++)
            {
                ChatLocated results_element;
                results_element = TL.Parse<ChatLocated>(reader);
                results.Add(results_element);
            }
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<GeoChatMessage>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                GeoChatMessage messages_element;
                messages_element = TL.Parse<GeoChatMessage>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(geochats_located results:{Serializers.VectorToString(results)} messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Geochats_messagesConstructor : geochats_Messages
    {
        public List<GeoChatMessage> messages;
        public List<Chat> chats;
        public List<User> users;

        public Geochats_messagesConstructor()
        {

        }

        public Geochats_messagesConstructor(List<GeoChatMessage> messages, List<Chat> chats, List<User> users)
        {
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.geochats_messages;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd1526db1);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (GeoChatMessage messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<GeoChatMessage>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                GeoChatMessage messages_element;
                messages_element = TL.Parse<GeoChatMessage>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
            }
        }

        public override string ToString()
        {
            return
                $"(geochats_messages messages:{Serializers.VectorToString(messages)} chats:{Serializers.VectorToString(chats)} users:{Serializers.VectorToString(users)})";
        }
    }


    public class Geochats_messagesSliceConstructor : geochats_Messages
    {
        public int count;
        public List<GeoChatMessage> messages;
        public List<Chat> chats;
        public List<User> users;

        public Geochats_messagesSliceConstructor()
        {

        }

        public Geochats_messagesSliceConstructor(int count, List<GeoChatMessage> messages, List<Chat> chats, List<User> users)
        {
            this.count = count;
            this.messages = messages;
            this.chats = chats;
            this.users = users;
        }


        public override Constructor Constructor => Constructor.geochats_messagesSlice;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xbc5863e8);
            writer.Write(count);
            writer.Write(0x1cb5c415);
            writer.Write(messages.Count);
            foreach (GeoChatMessage messages_element in messages)
            {
                messages_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(chats.Count);
            foreach (Chat chats_element in chats)
            {
                chats_element.Write(writer);
            }
            writer.Write(0x1cb5c415);
            writer.Write(users.Count);
            foreach (User users_element in users)
            {
                users_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            count = reader.ReadInt32();
            reader.ReadInt32(); // vector code
            int messages_len = reader.ReadInt32();
            messages = new List<GeoChatMessage>(messages_len);
            for (int messages_index = 0; messages_index < messages_len; messages_index++)
            {
                GeoChatMessage messages_element;
                messages_element = TL.Parse<GeoChatMessage>(reader);
                messages.Add(messages_element);
            }
            reader.ReadInt32(); // vector code
            int chats_len = reader.ReadInt32();
            chats = new List<Chat>(chats_len);
            for (int chats_index = 0; chats_index < chats_len; chats_index++)
            {
                Chat chats_element;
                chats_element = TL.Parse<Chat>(reader);
                chats.Add(chats_element);
            }
            reader.ReadInt32(); // vector code
            int users_len = reader.ReadInt32();
            users = new List<User>(users_len);
            for (int users_index = 0; users_index < users_len; users_index++)
            {
                User users_element;
                users_element = TL.Parse<User>(reader);
                users.Add(users_element);
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


        public override Constructor Constructor => Constructor.messageActionGeoChatCreate;

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

        public MessageActionGeoChatCheckinConstructor()
        {

        }



        public override Constructor Constructor => Constructor.messageActionGeoChatCheckin;

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


        public override Constructor Constructor => Constructor.updateNewGeoChatMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5a68e3f7);
            message.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<GeoChatMessage>(reader);
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
        public int bg_color;
        public int color;

        public WallPaperSolidConstructor()
        {

        }

        public WallPaperSolidConstructor(int id, string title, int bg_color, int color)
        {
            this.id = id;
            this.title = title;
            this.bg_color = bg_color;
            this.color = color;
        }


        public override Constructor Constructor => Constructor.wallPaperSolid;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x63117f24);
            writer.Write(id);
            Serializers.String.Write(writer, title);
            writer.Write(bg_color);
            writer.Write(color);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            title = Serializers.String.Read(reader);
            bg_color = reader.ReadInt32();
            color = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(wallPaperSolid id:{id} title:'{title}' bg_color:{bg_color} color:{color})";
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


        public override Constructor Constructor => Constructor.updateNewEncryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x12bcbd9a);
            message.Write(writer);
            writer.Write(qts);
        }

        public override void Read(BinaryReader reader)
        {
            message = TL.Parse<EncryptedMessage>(reader);
            qts = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateNewEncryptedMessage message:{message} qts:{qts})";
        }
    }


    public class UpdateEncryptedChatTypingConstructor : Update
    {
        public int chat_id;

        public UpdateEncryptedChatTypingConstructor()
        {

        }

        public UpdateEncryptedChatTypingConstructor(int chat_id)
        {
            this.chat_id = chat_id;
        }


        public override Constructor Constructor => Constructor.updateEncryptedChatTyping;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1710f156);
            writer.Write(chat_id);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateEncryptedChatTyping chat_id:{chat_id})";
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


        public override Constructor Constructor => Constructor.updateEncryption;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb4a2e88d);
            chat.Write(writer);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            chat = TL.Parse<EncryptedChat>(reader);
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateEncryption chat:{chat} date:{date})";
        }
    }


    public class UpdateEncryptedMessagesReadConstructor : Update
    {
        public int chat_id;
        public int max_date;
        public int date;

        public UpdateEncryptedMessagesReadConstructor()
        {

        }

        public UpdateEncryptedMessagesReadConstructor(int chat_id, int max_date, int date)
        {
            this.chat_id = chat_id;
            this.max_date = max_date;
            this.date = date;
        }


        public override Constructor Constructor => Constructor.updateEncryptedMessagesRead;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x38fe25b7);
            writer.Write(chat_id);
            writer.Write(max_date);
            writer.Write(date);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            max_date = reader.ReadInt32();
            date = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateEncryptedMessagesRead chat_id:{chat_id} max_date:{max_date} date:{date})";
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


        public override Constructor Constructor => Constructor.encryptedChatEmpty;

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
        public long access_hash;
        public int date;
        public int admin_id;
        public int participant_id;

        public EncryptedChatWaitingConstructor()
        {

        }

        public EncryptedChatWaitingConstructor(int id, long access_hash, int date, int admin_id, int participant_id)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.date = date;
            this.admin_id = admin_id;
            this.participant_id = participant_id;
        }


        public override Constructor Constructor => Constructor.encryptedChatWaiting;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3bf703dc);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(date);
            writer.Write(admin_id);
            writer.Write(participant_id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
            date = reader.ReadInt32();
            admin_id = reader.ReadInt32();
            participant_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(encryptedChatWaiting id:{id} access_hash:{access_hash} date:{date} admin_id:{admin_id} participant_id:{participant_id})";
        }
    }


    public class EncryptedChatRequestedConstructor : EncryptedChat
    {
        public int id;
        public long access_hash;
        public int date;
        public int admin_id;
        public int participant_id;
        public byte[] g_a;
        public byte[] nonce;

        public EncryptedChatRequestedConstructor()
        {

        }

        public EncryptedChatRequestedConstructor(int id, long access_hash, int date, int admin_id, int participant_id,
            byte[] g_a, byte[] nonce)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.date = date;
            this.admin_id = admin_id;
            this.participant_id = participant_id;
            this.g_a = g_a;
            this.nonce = nonce;
        }


        public override Constructor Constructor => Constructor.encryptedChatRequested;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xfda9a7b7);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(date);
            writer.Write(admin_id);
            writer.Write(participant_id);
            Serializers.Bytes.Write(writer, g_a);
            Serializers.Bytes.Write(writer, nonce);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
            date = reader.ReadInt32();
            admin_id = reader.ReadInt32();
            participant_id = reader.ReadInt32();
            g_a = Serializers.Bytes.Read(reader);
            nonce = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(encryptedChatRequested id:{id} access_hash:{access_hash} date:{date} admin_id:{admin_id} participant_id:{participant_id} g_a:{BitConverter.ToString(g_a)} nonce:{BitConverter.ToString(nonce)})";
        }
    }


    public class EncryptedChatConstructor : EncryptedChat
    {
        public int id;
        public long access_hash;
        public int date;
        public int admin_id;
        public int participant_id;
        public byte[] g_a_or_b;
        public byte[] nonce;
        public long key_fingerprint;

        public EncryptedChatConstructor()
        {

        }

        public EncryptedChatConstructor(int id, long access_hash, int date, int admin_id, int participant_id, byte[] g_a_or_b,
            byte[] nonce, long key_fingerprint)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.date = date;
            this.admin_id = admin_id;
            this.participant_id = participant_id;
            this.g_a_or_b = g_a_or_b;
            this.nonce = nonce;
            this.key_fingerprint = key_fingerprint;
        }


        public override Constructor Constructor => Constructor.encryptedChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6601d14f);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(date);
            writer.Write(admin_id);
            writer.Write(participant_id);
            Serializers.Bytes.Write(writer, g_a_or_b);
            Serializers.Bytes.Write(writer, nonce);
            writer.Write(key_fingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
            date = reader.ReadInt32();
            admin_id = reader.ReadInt32();
            participant_id = reader.ReadInt32();
            g_a_or_b = Serializers.Bytes.Read(reader);
            nonce = Serializers.Bytes.Read(reader);
            key_fingerprint = reader.ReadInt64();
        }

        public override string ToString()
        {
            return
                $"(encryptedChat id:{id} access_hash:{access_hash} date:{date} admin_id:{admin_id} participant_id:{participant_id} g_a_or_b:{BitConverter.ToString(g_a_or_b)} nonce:{BitConverter.ToString(nonce)} key_fingerprint:{key_fingerprint})";
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


        public override Constructor Constructor => Constructor.encryptedChatDiscarded;

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
        public int chat_id;
        public long access_hash;

        public InputEncryptedChatConstructor()
        {

        }

        public InputEncryptedChatConstructor(int chat_id, long access_hash)
        {
            this.chat_id = chat_id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputEncryptedChat;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf141b5e1);
            writer.Write(chat_id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputEncryptedChat chat_id:{chat_id} access_hash:{access_hash})";
        }
    }


    public class EncryptedFileEmptyConstructor : EncryptedFile
    {

        public EncryptedFileEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.encryptedFileEmpty;

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
        public long access_hash;
        public int size;
        public int dc_id;
        public int key_fingerprint;

        public EncryptedFileConstructor()
        {

        }

        public EncryptedFileConstructor(long id, long access_hash, int size, int dc_id, int key_fingerprint)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.size = size;
            this.dc_id = dc_id;
            this.key_fingerprint = key_fingerprint;
        }


        public override Constructor Constructor => Constructor.encryptedFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4a70994c);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(size);
            writer.Write(dc_id);
            writer.Write(key_fingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
            size = reader.ReadInt32();
            dc_id = reader.ReadInt32();
            key_fingerprint = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(encryptedFile id:{id} access_hash:{access_hash} size:{size} dc_id:{dc_id} key_fingerprint:{key_fingerprint})";
        }
    }


    public class InputEncryptedFileEmptyConstructor : InputEncryptedFile
    {

        public InputEncryptedFileEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputEncryptedFileEmpty;

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
        public string md5_checksum;
        public int key_fingerprint;

        public InputEncryptedFileUploadedConstructor()
        {

        }

        public InputEncryptedFileUploadedConstructor(long id, int parts, string md5_checksum, int key_fingerprint)
        {
            this.id = id;
            this.parts = parts;
            this.md5_checksum = md5_checksum;
            this.key_fingerprint = key_fingerprint;
        }


        public override Constructor Constructor => Constructor.inputEncryptedFileUploaded;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x64bd0306);
            writer.Write(id);
            writer.Write(parts);
            Serializers.String.Write(writer, md5_checksum);
            writer.Write(key_fingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            md5_checksum = Serializers.String.Read(reader);
            key_fingerprint = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(inputEncryptedFileUploaded id:{id} parts:{parts} md5_checksum:'{md5_checksum}' key_fingerprint:{key_fingerprint})";
        }
    }


    public class InputEncryptedFileConstructor : InputEncryptedFile
    {
        public long id;
        public long access_hash;

        public InputEncryptedFileConstructor()
        {

        }

        public InputEncryptedFileConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputEncryptedFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x5a17b5e5);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputEncryptedFile id:{id} access_hash:{access_hash})";
        }
    }


    public class InputEncryptedFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long access_hash;

        public InputEncryptedFileLocationConstructor()
        {

        }

        public InputEncryptedFileLocationConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputEncryptedFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf5235d55);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputEncryptedFileLocation id:{id} access_hash:{access_hash})";
        }
    }


    public class EncryptedMessageConstructor : EncryptedMessage
    {
        public long random_id;
        public int chat_id;
        public int date;
        public byte[] bytes;
        public EncryptedFile file;

        public EncryptedMessageConstructor()
        {

        }

        public EncryptedMessageConstructor(long random_id, int chat_id, int date, byte[] bytes, EncryptedFile file)
        {
            this.random_id = random_id;
            this.chat_id = chat_id;
            this.date = date;
            this.bytes = bytes;
            this.file = file;
        }


        public override Constructor Constructor => Constructor.encryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xed18c118);
            writer.Write(random_id);
            writer.Write(chat_id);
            writer.Write(date);
            Serializers.Bytes.Write(writer, bytes);
            file.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            random_id = reader.ReadInt64();
            chat_id = reader.ReadInt32();
            date = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
            file = TL.Parse<EncryptedFile>(reader);
        }

        public override string ToString()
        {
            return
                $"(encryptedMessage random_id:{random_id} chat_id:{chat_id} date:{date} bytes:{BitConverter.ToString(bytes)} file:{file})";
        }
    }


    public class EncryptedMessageServiceConstructor : EncryptedMessage
    {
        public long random_id;
        public int chat_id;
        public int date;
        public byte[] bytes;

        public EncryptedMessageServiceConstructor()
        {

        }

        public EncryptedMessageServiceConstructor(long random_id, int chat_id, int date, byte[] bytes)
        {
            this.random_id = random_id;
            this.chat_id = chat_id;
            this.date = date;
            this.bytes = bytes;
        }


        public override Constructor Constructor => Constructor.encryptedMessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x23734b06);
            writer.Write(random_id);
            writer.Write(chat_id);
            writer.Write(date);
            Serializers.Bytes.Write(writer, bytes);
        }

        public override void Read(BinaryReader reader)
        {
            random_id = reader.ReadInt64();
            chat_id = reader.ReadInt32();
            date = reader.ReadInt32();
            bytes = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(encryptedMessageService random_id:{random_id} chat_id:{chat_id} date:{date} bytes:{BitConverter.ToString(bytes)})";
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


        public override Constructor Constructor => Constructor.decryptedMessageLayer;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x99a438cf);
            writer.Write(layer);
            message.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            layer = reader.ReadInt32();
            message = TL.Parse<DecryptedMessage>(reader);
        }

        public override string ToString()
        {
            return $"(decryptedMessageLayer layer:{layer} message:{message})";
        }
    }


    public class DecryptedMessageConstructor : DecryptedMessage
    {
        public long random_id;
        public byte[] random_bytes;
        public string message;
        public DecryptedMessageMedia media;

        public DecryptedMessageConstructor()
        {

        }

        public DecryptedMessageConstructor(long random_id, byte[] random_bytes, string message, DecryptedMessageMedia media)
        {
            this.random_id = random_id;
            this.random_bytes = random_bytes;
            this.message = message;
            this.media = media;
        }


        public override Constructor Constructor => Constructor.decryptedMessage;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x1f814f1f);
            writer.Write(random_id);
            Serializers.Bytes.Write(writer, random_bytes);
            Serializers.String.Write(writer, message);
            media.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            random_id = reader.ReadInt64();
            random_bytes = Serializers.Bytes.Read(reader);
            message = Serializers.String.Read(reader);
            media = TL.Parse<DecryptedMessageMedia>(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessage random_id:{random_id} random_bytes:{BitConverter.ToString(random_bytes)} message:'{message}' media:{media})";
        }
    }


    public class DecryptedMessageServiceConstructor : DecryptedMessage
    {
        public long random_id;
        public byte[] random_bytes;
        public DecryptedMessageAction action;

        public DecryptedMessageServiceConstructor()
        {

        }

        public DecryptedMessageServiceConstructor(long random_id, byte[] random_bytes, DecryptedMessageAction action)
        {
            this.random_id = random_id;
            this.random_bytes = random_bytes;
            this.action = action;
        }


        public override Constructor Constructor => Constructor.decryptedMessageService;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xaa48327d);
            writer.Write(random_id);
            Serializers.Bytes.Write(writer, random_bytes);
            action.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            random_id = reader.ReadInt64();
            random_bytes = Serializers.Bytes.Read(reader);
            action = TL.Parse<DecryptedMessageAction>(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageService random_id:{random_id} random_bytes:{BitConverter.ToString(random_bytes)} action:{action})";
        }
    }


    public class DecryptedMessageMediaEmptyConstructor : DecryptedMessageMedia
    {

        public DecryptedMessageMediaEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.decryptedMessageMediaEmpty;

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
        public int thumb_w;
        public int thumb_h;
        public int w;
        public int h;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaPhotoConstructor()
        {

        }

        public DecryptedMessageMediaPhotoConstructor(byte[] thumb, int thumb_w, int thumb_h, int w, int h, int size,
            byte[] key, byte[] iv)
        {
            this.thumb = thumb;
            this.thumb_w = thumb_w;
            this.thumb_h = thumb_h;
            this.w = w;
            this.h = h;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor Constructor => Constructor.decryptedMessageMediaPhoto;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x32798a8c);
            Serializers.Bytes.Write(writer, thumb);
            writer.Write(thumb_w);
            writer.Write(thumb_h);
            writer.Write(w);
            writer.Write(h);
            writer.Write(size);
            Serializers.Bytes.Write(writer, key);
            Serializers.Bytes.Write(writer, iv);
        }

        public override void Read(BinaryReader reader)
        {
            thumb = Serializers.Bytes.Read(reader);
            thumb_w = reader.ReadInt32();
            thumb_h = reader.ReadInt32();
            w = reader.ReadInt32();
            h = reader.ReadInt32();
            size = reader.ReadInt32();
            key = Serializers.Bytes.Read(reader);
            iv = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaPhoto thumb:{BitConverter.ToString(thumb)} thumb_w:{thumb_w} thumb_h:{thumb_h} w:{w} h:{h} size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
        }
    }


    public class DecryptedMessageMediaVideoConstructor : DecryptedMessageMedia
    {
        public byte[] thumb;
        public int thumb_w;
        public int thumb_h;
        public int duration;
        public int w;
        public int h;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaVideoConstructor()
        {

        }

        public DecryptedMessageMediaVideoConstructor(byte[] thumb, int thumb_w, int thumb_h, int duration, int w, int h,
            int size, byte[] key, byte[] iv)
        {
            this.thumb = thumb;
            this.thumb_w = thumb_w;
            this.thumb_h = thumb_h;
            this.duration = duration;
            this.w = w;
            this.h = h;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor Constructor => Constructor.decryptedMessageMediaVideo;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4cee6ef3);
            Serializers.Bytes.Write(writer, thumb);
            writer.Write(thumb_w);
            writer.Write(thumb_h);
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
            thumb_w = reader.ReadInt32();
            thumb_h = reader.ReadInt32();
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
                $"(decryptedMessageMediaVideo thumb:{BitConverter.ToString(thumb)} thumb_w:{thumb_w} thumb_h:{thumb_h} duration:{duration} w:{w} h:{h} size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
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


        public override Constructor Constructor => Constructor.decryptedMessageMediaGeoPoint;

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
        public string phone_number;
        public string first_name;
        public string last_name;
        public int user_id;

        public DecryptedMessageMediaContactConstructor()
        {

        }

        public DecryptedMessageMediaContactConstructor(string phone_number, string first_name, string last_name, int user_id)
        {
            this.phone_number = phone_number;
            this.first_name = first_name;
            this.last_name = last_name;
            this.user_id = user_id;
        }


        public override Constructor Constructor => Constructor.decryptedMessageMediaContact;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x588a0a97);
            Serializers.String.Write(writer, phone_number);
            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
            writer.Write(user_id);
        }

        public override void Read(BinaryReader reader)
        {
            phone_number = Serializers.String.Read(reader);
            first_name = Serializers.String.Read(reader);
            last_name = Serializers.String.Read(reader);
            user_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaContact phone_number:'{phone_number}' first_name:'{first_name}' last_name:'{last_name}' user_id:{user_id})";
        }
    }


    public class DecryptedMessageActionSetMessageTTLConstructor : DecryptedMessageAction
    {
        public int ttl_seconds;

        public DecryptedMessageActionSetMessageTTLConstructor()
        {

        }

        public DecryptedMessageActionSetMessageTTLConstructor(int ttl_seconds)
        {
            this.ttl_seconds = ttl_seconds;
        }


        public override Constructor Constructor => Constructor.decryptedMessageActionSetMessageTTL;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xa1733aec);
            writer.Write(ttl_seconds);
        }

        public override void Read(BinaryReader reader)
        {
            ttl_seconds = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(decryptedMessageActionSetMessageTTL ttl_seconds:{ttl_seconds})";
        }
    }


    public class Messages_dhConfigNotModifiedConstructor : messages_DhConfig
    {
        public byte[] random;

        public Messages_dhConfigNotModifiedConstructor()
        {

        }

        public Messages_dhConfigNotModifiedConstructor(byte[] random)
        {
            this.random = random;
        }


        public override Constructor Constructor => Constructor.messages_dhConfigNotModified;

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


    public class Messages_dhConfigConstructor : messages_DhConfig
    {
        public int g;
        public byte[] p;
        public int version;
        public byte[] random;

        public Messages_dhConfigConstructor()
        {

        }

        public Messages_dhConfigConstructor(int g, byte[] p, int version, byte[] random)
        {
            this.g = g;
            this.p = p;
            this.version = version;
            this.random = random;
        }


        public override Constructor Constructor => Constructor.messages_dhConfig;

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


    public class Messages_sentEncryptedMessageConstructor : messages_SentEncryptedMessage
    {
        public int date;

        public Messages_sentEncryptedMessageConstructor()
        {

        }

        public Messages_sentEncryptedMessageConstructor(int date)
        {
            this.date = date;
        }


        public override Constructor Constructor => Constructor.messages_sentEncryptedMessage;

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


    public class Messages_sentEncryptedFileConstructor : messages_SentEncryptedMessage
    {
        public int date;
        public EncryptedFile file;

        public Messages_sentEncryptedFileConstructor()
        {

        }

        public Messages_sentEncryptedFileConstructor(int date, EncryptedFile file)
        {
            this.date = date;
            this.file = file;
        }


        public override Constructor Constructor => Constructor.messages_sentEncryptedFile;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x9493ff32);
            writer.Write(date);
            file.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            date = reader.ReadInt32();
            file = TL.Parse<EncryptedFile>(reader);
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


        public override Constructor Constructor => Constructor.inputFileBig;

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
        public int key_fingerprint;

        public InputEncryptedFileBigUploadedConstructor()
        {

        }

        public InputEncryptedFileBigUploadedConstructor(long id, int parts, int key_fingerprint)
        {
            this.id = id;
            this.parts = parts;
            this.key_fingerprint = key_fingerprint;
        }


        public override Constructor Constructor => Constructor.inputEncryptedFileBigUploaded;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2dc173c8);
            writer.Write(id);
            writer.Write(parts);
            writer.Write(key_fingerprint);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            parts = reader.ReadInt32();
            key_fingerprint = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(inputEncryptedFileBigUploaded id:{id} parts:{parts} key_fingerprint:{key_fingerprint})";
        }
    }


    public class UpdateChatParticipantAddConstructor : Update
    {
        public int chat_id;
        public int user_id;
        public int inviter_id;
        public int version;

        public UpdateChatParticipantAddConstructor()
        {

        }

        public UpdateChatParticipantAddConstructor(int chat_id, int user_id, int inviter_id, int version)
        {
            this.chat_id = chat_id;
            this.user_id = user_id;
            this.inviter_id = inviter_id;
            this.version = version;
        }


        public override Constructor Constructor => Constructor.updateChatParticipantAdd;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3a0eeb22);
            writer.Write(chat_id);
            writer.Write(user_id);
            writer.Write(inviter_id);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            user_id = reader.ReadInt32();
            inviter_id = reader.ReadInt32();
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(updateChatParticipantAdd chat_id:{chat_id} user_id:{user_id} inviter_id:{inviter_id} version:{version})";
        }
    }


    public class UpdateChatParticipantDeleteConstructor : Update
    {
        public int chat_id;
        public int user_id;
        public int version;

        public UpdateChatParticipantDeleteConstructor()
        {

        }

        public UpdateChatParticipantDeleteConstructor(int chat_id, int user_id, int version)
        {
            this.chat_id = chat_id;
            this.user_id = user_id;
            this.version = version;
        }


        public override Constructor Constructor => Constructor.updateChatParticipantDelete;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x6e5f8c22);
            writer.Write(chat_id);
            writer.Write(user_id);
            writer.Write(version);
        }

        public override void Read(BinaryReader reader)
        {
            chat_id = reader.ReadInt32();
            user_id = reader.ReadInt32();
            version = reader.ReadInt32();
        }

        public override string ToString()
        {
            return $"(updateChatParticipantDelete chat_id:{chat_id} user_id:{user_id} version:{version})";
        }
    }


    public class UpdateDcOptionsConstructor : Update
    {
        public List<DcOption> dc_options;

        public UpdateDcOptionsConstructor()
        {

        }

        public UpdateDcOptionsConstructor(List<DcOption> dc_options)
        {
            this.dc_options = dc_options;
        }


        public override Constructor Constructor => Constructor.updateDcOptions;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x8e5e9873);
            writer.Write(0x1cb5c415);
            writer.Write(dc_options.Count);
            foreach (DcOption dc_options_element in dc_options)
            {
                dc_options_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            reader.ReadInt32(); // vector code
            int dc_options_len = reader.ReadInt32();
            dc_options = new List<DcOption>(dc_options_len);
            for (int dc_options_index = 0; dc_options_index < dc_options_len; dc_options_index++)
            {
                DcOption dc_options_element;
                dc_options_element = TL.Parse<DcOption>(reader);
                dc_options.Add(dc_options_element);
            }
        }

        public override string ToString()
        {
            return $"(updateDcOptions dc_options:{Serializers.VectorToString(dc_options)})";
        }
    }

    public class UpdateUserBlockedConstructor : Update
    {
        public int userId;
        public bool blocked;

        public override Constructor Constructor => Constructor.updateUserBlocked;

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

        public override Constructor Constructor => Constructor.updateNotifySettings;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            peer = TL.Parse<InputNotifyPeer>(reader);
            notifySettings = TL.Parse<PeerNotifySettings>(reader);
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

        public override Constructor Constructor => Constructor.updateServiceNotification;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            type = reader.ReadString();
            message = reader.ReadString();
            media = TL.Parse<MessageMedia>(reader);
            popup = reader.ReadBoolean();
        }

        public override string ToString()
        {
            return $"(updateServiceNotification type:{type}, message:{message}, media:{media}, popup:{popup})";
        }
    }

    public class UpdatePrivacyConstructor : Update
    {
        public override Constructor Constructor => Constructor.updatePrivacy;

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

        public override Constructor Constructor => Constructor.updateUserPhone;

        public override void Write(BinaryWriter writer)
        {
            throw new NotSupportedException("Write not supported");
        }

        public override void Read(BinaryReader reader)
        {
            userId = reader.ReadInt32();
            phone = reader.ReadString();
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


        public override Constructor Constructor => Constructor.inputMediaUploadedAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x61a6d436);
            file.Write(writer);
            writer.Write(duration);
        }

        public override void Read(BinaryReader reader)
        {
            file = TL.Parse<InputFile>(reader);
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


        public override Constructor Constructor => Constructor.inputMediaAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x89938781);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = TL.Parse<InputAudio>(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaAudio id:{id})";
        }
    }


    public class InputMediaUploadedDocumentConstructor : InputMedia
    {
        public InputFile file;
        public string file_name;
        public string mime_type;

        public InputMediaUploadedDocumentConstructor()
        {

        }

        public InputMediaUploadedDocumentConstructor(InputFile file, string file_name, string mime_type)
        {
            this.file = file;
            this.file_name = file_name;
            this.mime_type = mime_type;
        }


        public override Constructor Constructor => Constructor.inputMediaUploadedDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x34e794bd);
            file.Write(writer);
            Serializers.String.Write(writer, file_name);
            Serializers.String.Write(writer, mime_type);
        }

        public override void Read(BinaryReader reader)
        {
            file = TL.Parse<InputFile>(reader);
            file_name = Serializers.String.Read(reader);
            mime_type = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return $"(inputMediaUploadedDocument file:{file} file_name:'{file_name}' mime_type:'{mime_type}')";
        }
    }


    public class InputMediaUploadedThumbDocumentConstructor : InputMedia
    {
        public InputFile file;
        public InputFile thumb;
        public string file_name;
        public string mime_type;

        public InputMediaUploadedThumbDocumentConstructor()
        {

        }

        public InputMediaUploadedThumbDocumentConstructor(InputFile file, InputFile thumb, string file_name, string mime_type)
        {
            this.file = file;
            this.thumb = thumb;
            this.file_name = file_name;
            this.mime_type = mime_type;
        }


        public override Constructor Constructor => Constructor.inputMediaUploadedThumbDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x3e46de5d);
            file.Write(writer);
            thumb.Write(writer);
            Serializers.String.Write(writer, file_name);
            Serializers.String.Write(writer, mime_type);
        }

        public override void Read(BinaryReader reader)
        {
            file = TL.Parse<InputFile>(reader);
            thumb = TL.Parse<InputFile>(reader);
            file_name = Serializers.String.Read(reader);
            mime_type = Serializers.String.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(inputMediaUploadedThumbDocument file:{file} thumb:{thumb} file_name:'{file_name}' mime_type:'{mime_type}')";
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


        public override Constructor Constructor => Constructor.inputMediaDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xd184e841);
            id.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            id = TL.Parse<InputDocument>(reader);
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


        public override Constructor Constructor => Constructor.messageMediaDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x2fda2204);
            document.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            document = TL.Parse<Document>(reader);
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


        public override Constructor Constructor => Constructor.messageMediaAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xc6b68300);
            audio.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            audio = TL.Parse<Audio>(reader);
        }

        public override string ToString()
        {
            return $"(messageMediaAudio audio:{audio})";
        }
    }


    public class InputAudioEmptyConstructor : InputAudio
    {

        public InputAudioEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputAudioEmpty;

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
        public long access_hash;

        public InputAudioConstructor()
        {

        }

        public InputAudioConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputAudio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x77d440ff);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputAudio id:{id} access_hash:{access_hash})";
        }
    }


    public class InputDocumentEmptyConstructor : InputDocument
    {

        public InputDocumentEmptyConstructor()
        {

        }



        public override Constructor Constructor => Constructor.inputDocumentEmpty;

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
        public long access_hash;

        public InputDocumentConstructor()
        {

        }

        public InputDocumentConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x18798952);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputDocument id:{id} access_hash:{access_hash})";
        }
    }


    public class InputAudioFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long access_hash;

        public InputAudioFileLocationConstructor()
        {

        }

        public InputAudioFileLocationConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputAudioFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x74dc404d);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputAudioFileLocation id:{id} access_hash:{access_hash})";
        }
    }


    public class InputDocumentFileLocationConstructor : InputFileLocation
    {
        public long id;
        public long access_hash;

        public InputDocumentFileLocationConstructor()
        {

        }

        public InputDocumentFileLocationConstructor(long id, long access_hash)
        {
            this.id = id;
            this.access_hash = access_hash;
        }


        public override Constructor Constructor => Constructor.inputDocumentFileLocation;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x4e45abe9);
            writer.Write(id);
            writer.Write(access_hash);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
        }

        public override string ToString()
        {
            return $"(inputDocumentFileLocation id:{id} access_hash:{access_hash})";
        }
    }


    public class DecryptedMessageMediaDocumentConstructor : DecryptedMessageMedia
    {
        public byte[] thumb;
        public int thumb_w;
        public int thumb_h;
        public string file_name;
        public string mime_type;
        public int size;
        public byte[] key;
        public byte[] iv;

        public DecryptedMessageMediaDocumentConstructor()
        {

        }

        public DecryptedMessageMediaDocumentConstructor(byte[] thumb, int thumb_w, int thumb_h, string file_name,
            string mime_type, int size, byte[] key, byte[] iv)
        {
            this.thumb = thumb;
            this.thumb_w = thumb_w;
            this.thumb_h = thumb_h;
            this.file_name = file_name;
            this.mime_type = mime_type;
            this.size = size;
            this.key = key;
            this.iv = iv;
        }


        public override Constructor Constructor => Constructor.decryptedMessageMediaDocument;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xb095434b);
            Serializers.Bytes.Write(writer, thumb);
            writer.Write(thumb_w);
            writer.Write(thumb_h);
            Serializers.String.Write(writer, file_name);
            Serializers.String.Write(writer, mime_type);
            writer.Write(size);
            Serializers.Bytes.Write(writer, key);
            Serializers.Bytes.Write(writer, iv);
        }

        public override void Read(BinaryReader reader)
        {
            thumb = Serializers.Bytes.Read(reader);
            thumb_w = reader.ReadInt32();
            thumb_h = reader.ReadInt32();
            file_name = Serializers.String.Read(reader);
            mime_type = Serializers.String.Read(reader);
            size = reader.ReadInt32();
            key = Serializers.Bytes.Read(reader);
            iv = Serializers.Bytes.Read(reader);
        }

        public override string ToString()
        {
            return
                $"(decryptedMessageMediaDocument thumb:{BitConverter.ToString(thumb)} thumb_w:{thumb_w} thumb_h:{thumb_h} file_name:'{file_name}' mime_type:'{mime_type}' size:{size} key:{BitConverter.ToString(key)} iv:{BitConverter.ToString(iv)})";
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


        public override Constructor Constructor => Constructor.decryptedMessageMediaAudio;

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


        public override Constructor Constructor => Constructor.audioEmpty;

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
        public long access_hash;
        public int user_id;
        public int date;
        public int duration;
        public int size;
        public int dc_id;

        public AudioConstructor()
        {

        }

        public AudioConstructor(long id, long access_hash, int user_id, int date, int duration, int size, int dc_id)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.user_id = user_id;
            this.date = date;
            this.duration = duration;
            this.size = size;
            this.dc_id = dc_id;
        }


        public override Constructor Constructor => Constructor.audio;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x427425e7);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(user_id);
            writer.Write(date);
            writer.Write(duration);
            writer.Write(size);
            writer.Write(dc_id);
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
            user_id = reader.ReadInt32();
            date = reader.ReadInt32();
            duration = reader.ReadInt32();
            size = reader.ReadInt32();
            dc_id = reader.ReadInt32();
        }

        public override string ToString()
        {
            return
                $"(audio id:{id} access_hash:{access_hash} user_id:{user_id} date:{date} duration:{duration} size:{size} dc_id:{dc_id})";
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


        public override Constructor Constructor => Constructor.documentEmpty;

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
        public long access_hash;
        public int date;
        public string mime_type;
        public int size;
        public PhotoSize thumb;
        public int dc_id;
        public List<DocumentAttribute> attributes;

        public DocumentConstructor()
        {

        }

        public DocumentConstructor(long id, long access_hash, int date, string mime_type,
            int size, PhotoSize thumb, int dc_id, List<DocumentAttribute> attributes)
        {
            this.id = id;
            this.access_hash = access_hash;
            this.date = date;
            this.mime_type = mime_type;
            this.size = size;
            this.thumb = thumb;
            this.dc_id = dc_id;
            this.attributes = attributes;
        }


        public override Constructor Constructor => Constructor.document;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0xf9a39f4f);
            writer.Write(id);
            writer.Write(access_hash);
            writer.Write(date);
            Serializers.String.Write(writer, mime_type);
            writer.Write(size);
            thumb.Write(writer);
            writer.Write(dc_id);

            writer.Write(0x1cb5c415);
            writer.Write(attributes.Count);
            foreach (DocumentAttribute attribute_element in attributes)
            {
                attribute_element.Write(writer);
            }
        }

        public override void Read(BinaryReader reader)
        {
            id = reader.ReadInt64();
            access_hash = reader.ReadInt64();
            date = reader.ReadInt32();
            mime_type = Serializers.String.Read(reader);
            size = reader.ReadInt32();
            thumb = TL.Parse<PhotoSize>(reader);
            dc_id = reader.ReadInt32();

            reader.ReadUInt32(); // vector code
            int attributes_len = reader.ReadInt32();
            attributes = new List<DocumentAttribute>(attributes_len);
            for (int attributes_index = 0; attributes_index < attributes_len; attributes_index++)
            {
                DocumentAttribute attribute_element;
                attribute_element = TL.Parse<DocumentAttribute>(reader);
                attributes.Add(attribute_element);
            }

            // stickers appear to have a superfluous 000 at the end of the stream;
            // unclear if this matches the 3 DocumentAttributes or is always just 000,
            // but there should be nothing after 'attributes' according to the spec.
            // we've got all the data we need for this document, so keep reading
            // until we locate the next constructor
            while (reader.BaseStream.Length != reader.BaseStream.Position && reader.PeekChar() == 0)
            {
                reader.ReadChar();
            }
        }

        public override string ToString()
        {
            return
                $"(document id:{id} access_hash:{access_hash} date:{date} mime_type:'{mime_type}' size:{size} thumb:{thumb} dc_id:{dc_id})";
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

        public override Constructor Constructor => Constructor.documentAttributeImageSize;

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
        public override Constructor Constructor => Constructor.documentAttributeAnimated;

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
        public override Constructor Constructor => Constructor.documentAttributeSticker;

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

        public override Constructor Constructor => Constructor.documentAttributeVideo;

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

        public override Constructor Constructor => Constructor.documentAttributeAudio;

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
        public string file_name;

        public DocumentAttributeFilenameConstructor()
        {
        }

        public DocumentAttributeFilenameConstructor(string file_name)
        {
            this.file_name = file_name;
        }

        public override Constructor Constructor => Constructor.documentAttributeFilename;

        public override void Write(BinaryWriter writer)
        {
            writer.Write(0x15590068);
            writer.Write(file_name);
        }

        public override void Read(BinaryReader reader)
        {
            file_name = reader.ReadString();
        }
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
