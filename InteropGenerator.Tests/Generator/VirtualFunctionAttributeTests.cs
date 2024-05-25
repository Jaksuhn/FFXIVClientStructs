﻿using InteropGenerator.Tests.Helpers;
using Xunit;
using VerifyIG = InteropGenerator.Tests.Helpers.IncrementalGeneratorVerifier<InteropGenerator.Generator.InteropGenerator>;

namespace InteropGenerator.Tests.Generator;

public class VirtualFunctionAttributeTests {
    [Fact]
    public async Task GenerateVirtualFunction() {
        const string code = """
                            [StructLayout(LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [StructLayout(LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [FieldOffset(40)] public delegate* unmanaged[Stdcall] <TestStruct*, int, void*, int> TestFunction;
                                  }
                                  [FieldOffset(0)] public TestStructVirtualTable* VirtualTable;
                                  [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)Unsafe.AsPointer(ref this), argOne, argTwo);
                              }
                              """;
        
        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }
    
    [Fact]
    public async Task GenerateVirtualFunctionNoReturn() {
        const string code = """
                            [StructLayout(LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial void TestFunction(int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [StructLayout(LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [FieldOffset(40)] public delegate* unmanaged[Stdcall] <TestStruct*, int, void*, void> TestFunction;
                                  }
                                  [FieldOffset(0)] public TestStructVirtualTable* VirtualTable;
                                  [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                  public partial void TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)Unsafe.AsPointer(ref this), argOne, argTwo);
                              }
                              """;
        
        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }
    
    [Fact]
    public async Task GenerateVirtualFunctionInNamespace() {
        const string code = """
                            namespace TestNamespace.InnerNamespace;
                            
                            [StructLayout(LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              namespace TestNamespace.InnerNamespace;
                              
                              unsafe partial struct TestStruct
                              {
                                  [StructLayout(LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [FieldOffset(40)] public delegate* unmanaged[Stdcall] <TestStruct*, int, void*, int> TestFunction;
                                  }
                                  [FieldOffset(0)] public TestStructVirtualTable* VirtualTable;
                                  [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)Unsafe.AsPointer(ref this), argOne, argTwo);
                              }
                              """;
        
        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestNamespace.InnerNamespace.TestStruct.InteropGenerator.g.cs", result));
    }
    
    [Fact]
    public async Task GenerateVirtualFunctionInnerStruct() {
        const string code = """
                            public unsafe partial struct TestStruct
                            {
                                [StructLayout(LayoutKind.Explicit)]
                                [GenerateInterop]
                                public unsafe partial struct InnerStruct
                                {
                                    [VirtualFunction(5)]
                                    public partial int TestFunction(int argOne, void * argTwo);
                                }
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  unsafe partial struct InnerStruct
                                  {
                                      [StructLayout(LayoutKind.Explicit)]
                                      public unsafe partial struct InnerStructVirtualTable
                                      {
                                          [FieldOffset(40)] public delegate* unmanaged[Stdcall] <InnerStruct*, int, void*, int> TestFunction;
                                      }
                                      [FieldOffset(0)] public InnerStructVirtualTable* VirtualTable;
                                      [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                      public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((InnerStruct*)Unsafe.AsPointer(ref this), argOne, argTwo);
                                  }
                              }
                              """;
        
        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct+InnerStruct.InteropGenerator.g.cs", result));
    }
    
    [Fact]
    public async Task GenerateVirtualFunctionParamModifier() {
        const string code = """
                            [StructLayout(LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(out int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [StructLayout(LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [FieldOffset(40)] public delegate* unmanaged[Stdcall] <TestStruct*, out int, void*, int> TestFunction;
                                  }
                                  [FieldOffset(0)] public TestStructVirtualTable* VirtualTable;
                                  [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(out int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)Unsafe.AsPointer(ref this), out argOne, argTwo);
                              }
                              """;
        
        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }    
    
    [Fact]
    public async Task GenerateVirtualFunctionMultiple() {
        const string code = """
                            [StructLayout(LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(int argOne, void * argTwo);
                                
                                [VirtualFunction(17)]
                                public partial void TestFunction2();
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [StructLayout(LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [FieldOffset(40)] public delegate* unmanaged[Stdcall] <TestStruct*, int, void*, int> TestFunction;
                                      [FieldOffset(136)] public delegate* unmanaged[Stdcall] <TestStruct*, void> TestFunction2;
                                  }
                                  [FieldOffset(0)] public TestStructVirtualTable* VirtualTable;
                                  [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)Unsafe.AsPointer(ref this), argOne, argTwo);
                                  [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                  public partial void TestFunction2() => VirtualTable->TestFunction2((TestStruct*)Unsafe.AsPointer(ref this));
                              }
                              """;
        
        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }    
}
