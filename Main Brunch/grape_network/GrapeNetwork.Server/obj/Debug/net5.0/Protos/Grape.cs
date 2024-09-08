// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/grape.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Grape {

  /// <summary>Holder for reflection information generated from Protos/grape.proto</summary>
  public static partial class GrapeReflection {

    #region Descriptor
    /// <summary>File descriptor for Protos/grape.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static GrapeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJQcm90b3MvZ3JhcGUucHJvdG8SBWdyYXBlIlMKFVJlcXVlc3RHZXREYXRh",
            "QWNjb3VudBIUCgxHcm91cENvbW1hbmQYASABKA0SDwoHQ29tbWFuZBgCIAEo",
            "DRITCgtOYW1lU2VydmljZRgDIAEoCSJUChZSZXNwb25zZUdldERhdGFBY2Nv",
            "dW50EhQKDEdyb3VwQ29tbWFuZBgBIAEoDRIPCgdDb21tYW5kGAIgASgNEhMK",
            "C05hbWVTZXJ2aWNlGAMgASgJMgYKBEdhbWUyBwoFTG9naW4yXQoIRGF0YWJh",
            "c2USUQoOR2V0QWNjb3VudERhdGESHC5ncmFwZS5SZXF1ZXN0R2V0RGF0YUFj",
            "Y291bnQaHS5ncmFwZS5SZXNwb25zZUdldERhdGFBY2NvdW50KAEwAWIGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Grape.RequestGetDataAccount), global::Grape.RequestGetDataAccount.Parser, new[]{ "GroupCommand", "Command", "NameService" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Grape.ResponseGetDataAccount), global::Grape.ResponseGetDataAccount.Parser, new[]{ "GroupCommand", "Command", "NameService" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class RequestGetDataAccount : pb::IMessage<RequestGetDataAccount>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RequestGetDataAccount> _parser = new pb::MessageParser<RequestGetDataAccount>(() => new RequestGetDataAccount());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RequestGetDataAccount> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Grape.GrapeReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestGetDataAccount() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestGetDataAccount(RequestGetDataAccount other) : this() {
      groupCommand_ = other.groupCommand_;
      command_ = other.command_;
      nameService_ = other.nameService_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RequestGetDataAccount Clone() {
      return new RequestGetDataAccount(this);
    }

    /// <summary>Field number for the "GroupCommand" field.</summary>
    public const int GroupCommandFieldNumber = 1;
    private uint groupCommand_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint GroupCommand {
      get { return groupCommand_; }
      set {
        groupCommand_ = value;
      }
    }

    /// <summary>Field number for the "Command" field.</summary>
    public const int CommandFieldNumber = 2;
    private uint command_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Command {
      get { return command_; }
      set {
        command_ = value;
      }
    }

    /// <summary>Field number for the "NameService" field.</summary>
    public const int NameServiceFieldNumber = 3;
    private string nameService_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string NameService {
      get { return nameService_; }
      set {
        nameService_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as RequestGetDataAccount);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(RequestGetDataAccount other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (GroupCommand != other.GroupCommand) return false;
      if (Command != other.Command) return false;
      if (NameService != other.NameService) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (GroupCommand != 0) hash ^= GroupCommand.GetHashCode();
      if (Command != 0) hash ^= Command.GetHashCode();
      if (NameService.Length != 0) hash ^= NameService.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (GroupCommand != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(GroupCommand);
      }
      if (Command != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Command);
      }
      if (NameService.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(NameService);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (GroupCommand != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(GroupCommand);
      }
      if (Command != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Command);
      }
      if (NameService.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(NameService);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (GroupCommand != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(GroupCommand);
      }
      if (Command != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Command);
      }
      if (NameService.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(NameService);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(RequestGetDataAccount other) {
      if (other == null) {
        return;
      }
      if (other.GroupCommand != 0) {
        GroupCommand = other.GroupCommand;
      }
      if (other.Command != 0) {
        Command = other.Command;
      }
      if (other.NameService.Length != 0) {
        NameService = other.NameService;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            GroupCommand = input.ReadUInt32();
            break;
          }
          case 16: {
            Command = input.ReadUInt32();
            break;
          }
          case 26: {
            NameService = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            GroupCommand = input.ReadUInt32();
            break;
          }
          case 16: {
            Command = input.ReadUInt32();
            break;
          }
          case 26: {
            NameService = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class ResponseGetDataAccount : pb::IMessage<ResponseGetDataAccount>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ResponseGetDataAccount> _parser = new pb::MessageParser<ResponseGetDataAccount>(() => new ResponseGetDataAccount());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ResponseGetDataAccount> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Grape.GrapeReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseGetDataAccount() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseGetDataAccount(ResponseGetDataAccount other) : this() {
      groupCommand_ = other.groupCommand_;
      command_ = other.command_;
      nameService_ = other.nameService_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ResponseGetDataAccount Clone() {
      return new ResponseGetDataAccount(this);
    }

    /// <summary>Field number for the "GroupCommand" field.</summary>
    public const int GroupCommandFieldNumber = 1;
    private uint groupCommand_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint GroupCommand {
      get { return groupCommand_; }
      set {
        groupCommand_ = value;
      }
    }

    /// <summary>Field number for the "Command" field.</summary>
    public const int CommandFieldNumber = 2;
    private uint command_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Command {
      get { return command_; }
      set {
        command_ = value;
      }
    }

    /// <summary>Field number for the "NameService" field.</summary>
    public const int NameServiceFieldNumber = 3;
    private string nameService_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string NameService {
      get { return nameService_; }
      set {
        nameService_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as ResponseGetDataAccount);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(ResponseGetDataAccount other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (GroupCommand != other.GroupCommand) return false;
      if (Command != other.Command) return false;
      if (NameService != other.NameService) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (GroupCommand != 0) hash ^= GroupCommand.GetHashCode();
      if (Command != 0) hash ^= Command.GetHashCode();
      if (NameService.Length != 0) hash ^= NameService.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (GroupCommand != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(GroupCommand);
      }
      if (Command != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Command);
      }
      if (NameService.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(NameService);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (GroupCommand != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(GroupCommand);
      }
      if (Command != 0) {
        output.WriteRawTag(16);
        output.WriteUInt32(Command);
      }
      if (NameService.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(NameService);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (GroupCommand != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(GroupCommand);
      }
      if (Command != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Command);
      }
      if (NameService.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(NameService);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(ResponseGetDataAccount other) {
      if (other == null) {
        return;
      }
      if (other.GroupCommand != 0) {
        GroupCommand = other.GroupCommand;
      }
      if (other.Command != 0) {
        Command = other.Command;
      }
      if (other.NameService.Length != 0) {
        NameService = other.NameService;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            GroupCommand = input.ReadUInt32();
            break;
          }
          case 16: {
            Command = input.ReadUInt32();
            break;
          }
          case 26: {
            NameService = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            GroupCommand = input.ReadUInt32();
            break;
          }
          case 16: {
            Command = input.ReadUInt32();
            break;
          }
          case 26: {
            NameService = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
