namespace FluorineFx.IO.Bytecode.Lightweight
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Exceptions;
    using System;
    using System.Diagnostics.SymbolStore;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;

    internal class EmitHelper
    {
        private readonly System.Reflection.Emit.ILGenerator _ilGenerator;
        public static BindingFlags AnyVisibilityInstance = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        public EmitHelper(System.Reflection.Emit.ILGenerator ilGenerator)
        {
            if (ilGenerator == null)
            {
                throw new ArgumentNullException("ilGenerator");
            }
            this._ilGenerator = ilGenerator;
        }

        public void AddMaxStackSize(int size)
        {
            FieldInfo field = this._ilGenerator.GetType().GetField("m_maxStackSize", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                size += (int)field.GetValue(this._ilGenerator);
                field.SetValue(this._ilGenerator, size);
            }
        }

        public EmitHelper BeginCatchBlock(Type exceptionType)
        {
            this._ilGenerator.BeginCatchBlock(exceptionType);
            return this;
        }

        public EmitHelper BeginExceptFilterBlock()
        {
            this._ilGenerator.BeginExceptFilterBlock();
            return this;
        }

        public Label BeginExceptionBlock()
        {
            return this._ilGenerator.BeginExceptionBlock();
        }

        public EmitHelper BeginFaultBlock()
        {
            this._ilGenerator.BeginFaultBlock();
            return this;
        }

        public EmitHelper BeginFinallyBlock()
        {
            this._ilGenerator.BeginFinallyBlock();
            return this;
        }

        public EmitHelper BeginScope()
        {
            this._ilGenerator.BeginScope();
            return this;
        }

        public EmitHelper beq(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Beq, label);
            return this;
        }

        public EmitHelper beq_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Beq_S, label);
            return this;
        }

        public EmitHelper bge(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bge, label);
            return this;
        }

        public EmitHelper bge_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bge_S, label);
            return this;
        }

        public EmitHelper bge_un(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bge_Un, label);
            return this;
        }

        public EmitHelper bge_un_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bge_Un_S, label);
            return this;
        }

        public EmitHelper bgt(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bgt, label);
            return this;
        }

        public EmitHelper bgt_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bgt_S, label);
            return this;
        }

        public EmitHelper bgt_un(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bgt_Un, label);
            return this;
        }

        public EmitHelper bgt_un_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bgt_Un_S, label);
            return this;
        }

        public EmitHelper ble(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Ble, label);
            return this;
        }

        public EmitHelper ble_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Ble_S, label);
            return this;
        }

        public EmitHelper ble_un(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Ble_Un, label);
            return this;
        }

        public EmitHelper ble_un_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Ble_Un_S, label);
            return this;
        }

        public EmitHelper blt(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Blt, label);
            return this;
        }

        public EmitHelper blt_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Blt_S, label);
            return this;
        }

        public EmitHelper blt_un(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Blt_Un, label);
            return this;
        }

        public EmitHelper blt_un_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Blt_Un_S, label);
            return this;
        }

        public EmitHelper bne_un(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bne_Un, label);
            return this;
        }

        public EmitHelper bne_un_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Bne_Un_S, label);
            return this;
        }

        public EmitHelper box(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Box, type);
            return this;
        }

        public EmitHelper boxIfValueType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return (type.IsValueType ? this.box(type) : this);
        }

        public EmitHelper br(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Br, label);
            return this;
        }

        public EmitHelper br_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Br_S, label);
            return this;
        }

        public EmitHelper brfalse(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Brfalse, label);
            return this;
        }

        public EmitHelper brfalse_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Brfalse_S, label);
            return this;
        }

        public EmitHelper brtrue(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Brtrue, label);
            return this;
        }

        public EmitHelper brtrue_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Brtrue_S, label);
            return this;
        }

        public EmitHelper call(ConstructorInfo constructorInfo)
        {
            this._ilGenerator.Emit(OpCodes.Call, constructorInfo);
            return this;
        }

        public EmitHelper call(MethodInfo methodInfo)
        {
            this._ilGenerator.Emit(OpCodes.Call, methodInfo);
            return this;
        }

        public EmitHelper call(MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            this._ilGenerator.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes);
            return this;
        }

        public EmitHelper call(Type type, string methodName, params Type[] parameterTypes)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo method = type.GetMethod(methodName, parameterTypes);
            return this.call(method);
        }

        public EmitHelper call(Type type, string methodName, BindingFlags flags, params Type[] parameterTypes)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo methodInfo = type.GetMethod(methodName, flags, null, parameterTypes, null);
            return this.call(methodInfo);
        }

        public EmitHelper calli(CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            this._ilGenerator.EmitCalli(OpCodes.Calli, unmanagedCallConv, returnType, parameterTypes);
            return this;
        }

        public EmitHelper calli(CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            this._ilGenerator.EmitCalli(OpCodes.Calli, callingConvention, returnType, parameterTypes, optionalParameterTypes);
            return this;
        }

        public EmitHelper callvirt(MethodInfo methodInfo)
        {
            this._ilGenerator.Emit(OpCodes.Callvirt, methodInfo);
            return this;
        }

        public EmitHelper callvirt(MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            this._ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, optionalParameterTypes);
            return this;
        }

        public EmitHelper callvirt(Type type, string methodName, params Type[] parameterTypes)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo method = type.GetMethod(methodName, parameterTypes);
            return this.callvirt(method);
        }

        public EmitHelper callvirt(Type type, string methodName, BindingFlags bindingAttr)
        {
            return this.callvirt(type, methodName, bindingAttr, null);
        }

        public EmitHelper callvirt(Type type, string methodName, BindingFlags bindingAttr, params Type[] optionalParameterTypes)
        {
            MethodInfo methodInfo = (optionalParameterTypes == null) ? type.GetMethod(methodName, bindingAttr) : type.GetMethod(methodName, bindingAttr, null, optionalParameterTypes, null);
            return this.callvirt(methodInfo, null);
        }

        public EmitHelper callvirtNoGenerics(Type type, string methodName, params Type[] parameterTypes)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, GenericBinder.NonGeneric, parameterTypes, null);
            return this.callvirt(methodInfo, (parameterTypes.Length == 0) ? null : parameterTypes);
        }

        public EmitHelper castclass(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Castclass, type);
            return this;
        }

        public EmitHelper CastFromObject(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type == typeof(object))
            {
                return this;
            }
            return (type.IsValueType ? this.unbox_any(type) : this.castclass(type));
        }

        public EmitHelper castType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return (type.IsValueType ? this.unbox(type) : this.castclass(type));
        }

        public EmitHelper conv(Type type)
        {
            if (type == typeof(byte))
            {
                this.conv_u1.end();
            }
            else if (type == typeof(char))
            {
                this.conv_u2.end();
            }
            else if (type == typeof(ushort))
            {
                this.conv_u2.end();
            }
            else if (type == typeof(uint))
            {
                this.conv_u4.end();
            }
            else if (type == typeof(ulong))
            {
                this.conv_i8.end();
            }
            else if (type == typeof(bool))
            {
                this.conv_i1.end();
            }
            else if (type == typeof(sbyte))
            {
                this.conv_i1.end();
            }
            else if (type == typeof(short))
            {
                this.conv_i2.end();
            }
            else if (type == typeof(int))
            {
                this.conv_i4.end();
            }
            else if (type == typeof(long))
            {
                this.conv_i8.end();
            }
            else if (type == typeof(float))
            {
                this.conv_r4.end();
            }
            else if (type == typeof(double))
            {
                this.conv_r8.end();
            }
            return this;
        }

        public EmitHelper cpobj(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Cpobj, type);
            return this;
        }

        public LocalBuilder DeclareLocal(Type localType)
        {
            return this._ilGenerator.DeclareLocal(localType);
        }

        public Label DefineLabel()
        {
            return this._ilGenerator.DefineLabel();
        }

        public void end()
        {
        }

        public EmitHelper EndExceptionBlock()
        {
            this._ilGenerator.EndExceptionBlock();
            return this;
        }

        public EmitHelper EndScope()
        {
            this._ilGenerator.EndScope();
            return this;
        }

        public EmitHelper GenerateNullValueAccess(MemberInfo memberInfo, Type memberType, Label labelStart)
        {
            MethodInfo method = typeof(FluorineConfiguration).GetMethod("get_Instance");
            MethodInfo methodInfo = typeof(FluorineConfiguration).GetMethod("get_NullableValues");
            MethodInfo info3 = typeof(Type).GetMethod("GetTypeFromHandle");
            MethodInfo info4 = typeof(NullableTypeCollection).GetMethod("get_Item", new Type[] { typeof(Type) });
            this.MarkLabel(labelStart).ldloc_0.call(method).callvirt(methodInfo).ldtoken(memberType).call(info3).callvirt(info4).unbox_any(memberType).end();
            return this;
        }

        public EmitHelper GeneratePrimitiveCast(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.SByte:
                    this.conv_i1.end();
                    break;

                case TypeCode.Byte:
                    this.conv_u1.end();
                    break;

                case TypeCode.Int16:
                    this.conv_i2.end();
                    break;

                case TypeCode.UInt16:
                    this.conv_u2.end();
                    break;

                case TypeCode.Int32:
                    this.conv_i4.end();
                    break;

                case TypeCode.UInt32:
                    this.conv_u4.end();
                    break;

                case TypeCode.Int64:
                    this.conv_i8.end();
                    break;

                case TypeCode.UInt64:
                    this.conv_u8.end();
                    break;

                case TypeCode.Single:
                    this.conv_r4.end();
                    break;

                case TypeCode.Double:
                    this.conv_r8.end();
                    break;

                case TypeCode.Decimal:
                    {
                        MethodInfo method = typeof(decimal).GetMethod("op_Explicit", new Type[] { typeof(double) });
                        this.conv_r8.call(method).end();
                        break;
                    }
            }
            return this;
        }

        public EmitHelper GenerateSetMember(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo field = memberInfo.DeclaringType.GetField(memberInfo.Name);
                return this.stfld(field);
            }
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                MethodInfo setMethod = memberInfo.DeclaringType.GetProperty(memberInfo.Name).GetSetMethod();
                return this.callvirt(setMethod);
            }
            return this;
        }

        public EmitHelper GenerateStoreElementFromObject(Type memberType)
        {
            switch (Type.GetTypeCode(memberType))
            {
                case TypeCode.Boolean:
                    this.unbox_any(typeof(double)).conv_i4.stelem_i4.end();
                    break;

                case TypeCode.Char:
                    throw new NotSupportedException();

                case TypeCode.SByte:
                    this.unbox_any(typeof(double)).conv_i1.stelem_i1.end();
                    break;

                case TypeCode.Byte:
                    this.unbox_any(typeof(double)).conv_u1.stelem_i1.end();
                    break;

                case TypeCode.Int16:
                    this.unbox_any(typeof(double)).conv_i2.stelem_i2.end();
                    break;

                case TypeCode.UInt16:
                    this.unbox_any(typeof(double)).conv_u2.stelem_i2.end();
                    break;

                case TypeCode.Int32:
                    this.unbox_any(typeof(double)).conv_i4.stelem_i4.end();
                    break;

                case TypeCode.UInt32:
                    this.unbox_any(typeof(double)).conv_u4.stelem_i4.end();
                    break;

                case TypeCode.Int64:
                    this.unbox_any(typeof(double)).conv_i8.stelem_i8.end();
                    break;

                case TypeCode.UInt64:
                    this.unbox_any(typeof(double)).conv_u8.stelem_i8.end();
                    break;

                case TypeCode.Single:
                    this.unbox_any(typeof(double)).conv_r4.stelem_r4.end();
                    break;

                case TypeCode.Double:
                    this.unbox_any(typeof(double)).conv_r8.stelem_r8.end();
                    break;

                case TypeCode.Decimal:
                    {
                        MethodInfo method = typeof(decimal).GetMethod("op_Explicit", new Type[] { typeof(double) });
                        this.unbox_any(typeof(double)).call(method).stobj(typeof(decimal)).end();
                        break;
                    }
                default:
                    this.stobj(memberType).end();
                    break;
            }
            return this;
        }

        public EmitHelper GenerateThrowUnexpectedAMFException(MemberInfo memberInfo)
        {
            ConstructorInfo constructorInfo = typeof(UnexpectedAMF).GetConstructor(AnyVisibilityInstance, null, CallingConventions.HasThis, new Type[] { typeof(string) }, null);
            this.ldstr(string.Format("Unexpected data for member {0}", memberInfo.Name)).newobj(constructorInfo).@throw.end();
            return this;
        }

        public EmitHelper Init(LocalBuilder localBuilder)
        {
            if (localBuilder == null)
            {
                throw new ArgumentNullException("localBuilder");
            }
            Type localType = localBuilder.LocalType;
            if (localType.IsEnum)
            {
                localType = Enum.GetUnderlyingType(localType);
            }
            return ((localType.IsValueType && !localType.IsPrimitive) ? this.ldloca(localBuilder).initobj(localType) : this.LoadInitValue(localType).stloc(localBuilder));
        }

        public EmitHelper Init(ParameterInfo parameterInfo, int index)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException("parameterInfo");
            }
            Type underlyingType = TypeHelper.GetUnderlyingType(parameterInfo.ParameterType);
            if (parameterInfo.ParameterType.IsByRef)
            {
                underlyingType = underlyingType.GetElementType();
                return ((underlyingType.IsValueType && !underlyingType.IsPrimitive) ? this.ldarg(index).initobj(underlyingType) : this.ldarg(index).LoadInitValue(underlyingType).stind(underlyingType));
            }
            return ((underlyingType.IsValueType && !underlyingType.IsPrimitive) ? this.ldarga(index).initobj(underlyingType) : this.LoadInitValue(underlyingType).starg(index));
        }

        public EmitHelper initobj(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Initobj, type);
            return this;
        }

        public EmitHelper InitOutParameters(ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameterInfo = parameters[i];
                if (parameterInfo.IsOut)
                {
                    this.Init(parameterInfo, i + 1);
                }
            }
            return this;
        }

        public EmitHelper isinst(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Isinst, type);
            return this;
        }

        public EmitHelper jmp(MethodInfo methodInfo)
        {
            this._ilGenerator.Emit(OpCodes.Jmp, methodInfo);
            return this;
        }

        public EmitHelper ldarg(short index)
        {
            this._ilGenerator.Emit(OpCodes.Ldarg, index);
            return this;
        }

        public EmitHelper ldarg(int index)
        {
            switch (index)
            {
                case 0:
                    this.ldarg_0.end();
                    break;

                case 1:
                    this.ldarg_1.end();
                    break;

                case 2:
                    this.ldarg_2.end();
                    break;

                case 3:
                    this.ldarg_3.end();
                    break;

                default:
                    if (index <= 0xff)
                    {
                        this.ldarg_s((byte)index);
                    }
                    else
                    {
                        if (index > 0x7fff)
                        {
                            throw new ArgumentOutOfRangeException("index");
                        }
                        this.ldarg((short)index);
                    }
                    break;
            }
            return this;
        }

        public EmitHelper ldarg_s(byte index)
        {
            this._ilGenerator.Emit(OpCodes.Ldarg_S, index);
            return this;
        }

        public EmitHelper ldarga(short index)
        {
            this._ilGenerator.Emit(OpCodes.Ldarga, index);
            return this;
        }

        public EmitHelper ldarga(int index)
        {
            if (index <= 0xff)
            {
                this.ldarga_s((byte)index);
            }
            else
            {
                if (index > 0x7fff)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                this.ldarga((short)index);
            }
            return this;
        }

        public EmitHelper ldarga_s(byte index)
        {
            this._ilGenerator.Emit(OpCodes.Ldarga_S, index);
            return this;
        }

        public EmitHelper ldc_i4(int num)
        {
            this._ilGenerator.Emit(OpCodes.Ldc_I4, num);
            return this;
        }

        public EmitHelper ldc_i4_(int num)
        {
            switch (num)
            {
                case -1:
                    this.ldc_i4_m1.end();
                    break;

                case 0:
                    this.ldc_i4_0.end();
                    break;

                case 1:
                    this.ldc_i4_1.end();
                    break;

                case 2:
                    this.ldc_i4_2.end();
                    break;

                case 3:
                    this.ldc_i4_3.end();
                    break;

                case 4:
                    this.ldc_i4_4.end();
                    break;

                case 5:
                    this.ldc_i4_5.end();
                    break;

                case 6:
                    this.ldc_i4_6.end();
                    break;

                case 7:
                    this.ldc_i4_7.end();
                    break;

                case 8:
                    this.ldc_i4_8.end();
                    break;

                default:
                    if ((num >= 0) && (num <= 0xff))
                    {
                        this.ldc_i4_s((byte)num);
                    }
                    else
                    {
                        this.ldc_i4(num);
                    }
                    break;
            }
            return this;
        }

        public EmitHelper ldc_i4_s(byte num)
        {
            this._ilGenerator.Emit(OpCodes.Ldc_I4_S, num);
            return this;
        }

        public EmitHelper ldc_i8(long num)
        {
            this._ilGenerator.Emit(OpCodes.Ldc_I8, num);
            return this;
        }

        public EmitHelper ldc_r4(float num)
        {
            this._ilGenerator.Emit(OpCodes.Ldc_R4, num);
            return this;
        }

        public EmitHelper ldc_r8(double num)
        {
            this._ilGenerator.Emit(OpCodes.Ldc_R8, num);
            return this;
        }

        public EmitHelper ldelema(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Ldelema, type);
            return this;
        }

        public EmitHelper ldfld(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            return this;
        }

        public EmitHelper ldflda(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldflda, fieldInfo);
            return this;
        }

        public EmitHelper ldftn(MethodInfo methodInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldftn, methodInfo);
            return this;
        }

        public EmitHelper ldind(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.IsClass)
            {
                this.ldind_ref.end();
            }
            else if (type == typeof(int))
            {
                this.ldind_i4.end();
            }
            else if (type == typeof(uint))
            {
                this.ldind_u4.end();
            }
            else if ((type == typeof(bool)) || (type == typeof(byte)))
            {
                this.ldind_u1.end();
            }
            else if (type == typeof(sbyte))
            {
                this.ldind_i1.end();
            }
            else if ((type == typeof(char)) || (type == typeof(ushort)))
            {
                this.ldind_u2.end();
            }
            else if (type == typeof(short))
            {
                this.ldind_i2.end();
            }
            else if (type == typeof(double))
            {
                this.ldind_r8.end();
            }
            else if (type == typeof(float))
            {
                this.ldind_r4.end();
            }
            else
            {
                if ((type != typeof(long)) && (type != typeof(ulong)))
                {
                    throw new InvalidOperationException();
                }
                this.ldind_i8.end();
            }
            return this;
        }

        public EmitHelper ldloc(short index)
        {
            this._ilGenerator.Emit(OpCodes.Ldloc, index);
            return this;
        }

        public EmitHelper ldloc(LocalBuilder localBuilder)
        {
            this._ilGenerator.Emit(OpCodes.Ldloc, localBuilder);
            return this;
        }

        public EmitHelper ldloc_s(byte index)
        {
            this._ilGenerator.Emit(OpCodes.Ldloca_S, index);
            return this;
        }

        public EmitHelper ldloca(short index)
        {
            this._ilGenerator.Emit(OpCodes.Ldloca, index);
            return this;
        }

        public EmitHelper ldloca(LocalBuilder local)
        {
            this._ilGenerator.Emit(OpCodes.Ldloca, local);
            return this;
        }

        public EmitHelper ldloca_s(byte index)
        {
            this._ilGenerator.Emit(OpCodes.Ldloca_S, index);
            return this;
        }

        public EmitHelper ldobj(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Ldobj, type);
            return this;
        }

        public EmitHelper ldsfld(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldsfld, fieldInfo);
            return this;
        }

        public EmitHelper ldsflda(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldsflda, fieldInfo);
            return this;
        }

        public EmitHelper ldstr(string str)
        {
            this._ilGenerator.Emit(OpCodes.Ldstr, str);
            return this;
        }

        public EmitHelper ldstrEx(string str)
        {
            return ((str == null) ? this.ldnull : this.ldstr(str));
        }

        public EmitHelper ldtoken(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldtoken, fieldInfo);
            return this;
        }

        public EmitHelper ldtoken(MethodInfo methodInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldtoken, methodInfo);
            return this;
        }

        public EmitHelper ldtoken(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Ldtoken, type);
            return this;
        }

        public EmitHelper ldvirtftn(MethodInfo methodInfo)
        {
            this._ilGenerator.Emit(OpCodes.Ldvirtftn, methodInfo);
            return this;
        }

        public EmitHelper leave(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Leave, label);
            return this;
        }

        public EmitHelper leave_s(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Leave_S, label);
            return this;
        }

        public EmitHelper LoadInitValue(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type == typeof(string))
            {
                this.ldsfld(typeof(string).GetField("Empty"));
            }
            else if (type.IsClass || type.IsInterface)
            {
                this.ldnull.end();
            }
            else if (((((type == typeof(bool)) || (type == typeof(byte))) || ((type == typeof(int)) || (type == typeof(uint)))) || (((type == typeof(short)) || (type == typeof(ushort))) || (type == typeof(sbyte)))) || (type == typeof(char)))
            {
                this.ldc_i4_0.end();
            }
            else if ((type == typeof(double)) || (type == typeof(float)))
            {
                this.ldc_r4(0f).end();
            }
            else
            {
                if ((type != typeof(long)) && (type != typeof(ulong)))
                {
                    throw new InvalidOperationException();
                }
                this.ldc_i4_0.conv_i8.end();
            }
            return this;
        }

        public EmitHelper LoadType(Type type)
        {
            return this.ldtoken(type).call(typeof(Type), "GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
        }

        public bool LoadWellKnownValue(object o)
        {
            if (o == null)
            {
                this.ldnull.end();
            }
            else if (o is string)
            {
                this.ldstr((string)o);
            }
            else if (o is int)
            {
                this.ldc_i4((int)o);
            }
            else if (o is byte)
            {
                this.ldc_i4((byte)o);
            }
            else if (o is sbyte)
            {
                this.ldc_i4((sbyte)o);
            }
            else if (o is char)
            {
                this.ldc_i4((char)o);
            }
            else if (o is ushort)
            {
                this.ldc_i4((ushort)o);
            }
            else if (o is uint)
            {
                this.ldc_i4((int)((uint)o));
            }
            else if (o is ulong)
            {
                this.ldc_i8((long)((ulong)o));
            }
            else if (o is bool)
            {
                this.ldc_i4(((bool)o) ? 1 : 0);
            }
            else if (o is short)
            {
                this.ldc_i4((short)o);
            }
            else if (o is long)
            {
                this.ldc_i8((long)o);
            }
            else if (o is float)
            {
                this.ldc_r4((float)o);
            }
            else if (o is double)
            {
                this.ldc_r8((double)o);
            }
            else
            {
                return false;
            }
            return true;
        }

        public EmitHelper MarkLabel(Label loc)
        {
            this._ilGenerator.MarkLabel(loc);
            return this;
        }

        public EmitHelper MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
            this._ilGenerator.MarkSequencePoint(document, startLine, startColumn, endLine, endColumn);
            return this;
        }

        public EmitHelper mkrefany(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Mkrefany, type);
            return this;
        }

        public EmitHelper newarr(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Newarr, type);
            return this;
        }

        public EmitHelper newobj(ConstructorInfo constructorInfo)
        {
            this._ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
            return this;
        }

        public EmitHelper newobj(Type type, params Type[] parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ConstructorInfo constructor = type.GetConstructor(parameters);
            return this.newobj(constructor);
        }

        public static implicit operator System.Reflection.Emit.ILGenerator(EmitHelper emitHelper)
        {
            if (emitHelper == null)
            {
                throw new ArgumentNullException("emitHelper");
            }
            return emitHelper.ILGenerator;
        }

        public EmitHelper refanyval(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Refanyval, type);
            return this;
        }

        public EmitHelper ret()
        {
            this._ilGenerator.Emit(OpCodes.Ret);
            return this;
        }

        public EmitHelper @sizeof(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Sizeof, type);
            return this;
        }

        public EmitHelper starg(short index)
        {
            this._ilGenerator.Emit(OpCodes.Starg, index);
            return this;
        }

        public EmitHelper starg(int index)
        {
            if (index < 0xff)
            {
                this.starg_s((byte)index);
            }
            else
            {
                if (index >= 0x7fff)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                this.starg((short)index);
            }
            return this;
        }

        public EmitHelper starg_s(byte index)
        {
            this._ilGenerator.Emit(OpCodes.Starg_S, index);
            return this;
        }

        public EmitHelper stfld(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Stfld, fieldInfo);
            return this;
        }

        public EmitHelper stind(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.IsClass)
            {
                this.stind_ref.end();
            }
            else if ((type == typeof(int)) || (type == typeof(uint)))
            {
                this.stind_i4.end();
            }
            else if (((type == typeof(bool)) || (type == typeof(byte))) || (type == typeof(sbyte)))
            {
                this.stind_i1.end();
            }
            else if (((type == typeof(char)) || (type == typeof(short))) || (type == typeof(ushort)))
            {
                this.stind_i2.end();
            }
            else if (type == typeof(double))
            {
                this.stind_r8.end();
            }
            else if (type == typeof(float))
            {
                this.stind_r4.end();
            }
            else if ((type == typeof(long)) || (type == typeof(ulong)))
            {
                this.stind_i8.end();
            }
            else
            {
                if (!type.IsValueType)
                {
                    throw new InvalidOperationException();
                }
                this.stobj(type);
            }
            return this;
        }

        public EmitHelper stloc(short index)
        {
            this._ilGenerator.Emit(OpCodes.Stloc, index);
            return this;
        }

        public EmitHelper stloc(LocalBuilder local)
        {
            this._ilGenerator.Emit(OpCodes.Stloc, local);
            return this;
        }

        public EmitHelper stloc_s(byte index)
        {
            this._ilGenerator.Emit(OpCodes.Stloc_S, index);
            return this;
        }

        public EmitHelper stloc_s(LocalBuilder local)
        {
            this._ilGenerator.Emit(OpCodes.Stloc_S, local);
            return this;
        }

        public EmitHelper stobj(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Stobj, type);
            return this;
        }

        public EmitHelper stsfld(FieldInfo fieldInfo)
        {
            this._ilGenerator.Emit(OpCodes.Stsfld, fieldInfo);
            return this;
        }

        public EmitHelper @switch(Label[] labels)
        {
            this._ilGenerator.Emit(OpCodes.Switch, labels);
            return this;
        }

        public EmitHelper ThrowException(Type exceptionType)
        {
            this._ilGenerator.ThrowException(exceptionType);
            return this;
        }

        public EmitHelper unaligned(long addr)
        {
            this._ilGenerator.Emit(OpCodes.Unaligned, addr);
            return this;
        }

        public EmitHelper unaligned(Label label)
        {
            this._ilGenerator.Emit(OpCodes.Unaligned, label);
            return this;
        }

        public EmitHelper unbox(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Unbox, type);
            return this;
        }

        public EmitHelper unbox_any(Type type)
        {
            this._ilGenerator.Emit(OpCodes.Unbox_Any, type);
            return this;
        }

        public EmitHelper unboxIfValueType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return (type.IsValueType ? this.unbox(type) : this);
        }

        public EmitHelper unboxOrCast(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return (type.IsValueType ? this.unbox(type) : this.castclass(type));
        }

        public EmitHelper UsingNamespace(string namespaceName)
        {
            this._ilGenerator.UsingNamespace(namespaceName);
            return this;
        }

        public EmitHelper add
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Add);
                return this;
            }
        }

        public EmitHelper add_ovf
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Add_Ovf);
                return this;
            }
        }

        public EmitHelper add_ovf_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Add_Ovf_Un);
                return this;
            }
        }

        public EmitHelper and
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.And);
                return this;
            }
        }

        public EmitHelper arglist
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Arglist);
                return this;
            }
        }

        public EmitHelper @break
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Break);
                return this;
            }
        }

        public EmitHelper ceq
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ceq);
                return this;
            }
        }

        public EmitHelper cgt
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Cgt);
                return this;
            }
        }

        public EmitHelper cgt_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Cgt_Un);
                return this;
            }
        }

        public EmitHelper ckfinite
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ckfinite);
                return this;
            }
        }

        public EmitHelper clt
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Clt);
                return this;
            }
        }

        public EmitHelper clt_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Clt_Un);
                return this;
            }
        }

        public EmitHelper conv_i
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_I);
                return this;
            }
        }

        public EmitHelper conv_i1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_I1);
                return this;
            }
        }

        public EmitHelper conv_i2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_I2);
                return this;
            }
        }

        public EmitHelper conv_i4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_I4);
                return this;
            }
        }

        public EmitHelper conv_i8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_I8);
                return this;
            }
        }

        public EmitHelper conv_ovf_i
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I);
                return this;
            }
        }

        public EmitHelper conv_ovf_i_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_i1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I1);
                return this;
            }
        }

        public EmitHelper conv_ovf_i1_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I1_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_i2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I2);
                return this;
            }
        }

        public EmitHelper conv_ovf_i2_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I2_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_i4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I2_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_i4_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I4_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_i8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I8);
                return this;
            }
        }

        public EmitHelper conv_ovf_i8_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_I8_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_u
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U);
                return this;
            }
        }

        public EmitHelper conv_ovf_u_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_u1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U1);
                return this;
            }
        }

        public EmitHelper conv_ovf_u1_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U1_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_u2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U2);
                return this;
            }
        }

        public EmitHelper conv_ovf_u2_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U2_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_u4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U4);
                return this;
            }
        }

        public EmitHelper conv_ovf_u4_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U4_Un);
                return this;
            }
        }

        public EmitHelper conv_ovf_u8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U8);
                return this;
            }
        }

        public EmitHelper conv_ovf_u8_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_Ovf_U8_Un);
                return this;
            }
        }

        public EmitHelper conv_r_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_R_Un);
                return this;
            }
        }

        public EmitHelper conv_r4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_R4);
                return this;
            }
        }

        public EmitHelper conv_r8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_R8);
                return this;
            }
        }

        public EmitHelper conv_u
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_U);
                return this;
            }
        }

        public EmitHelper conv_u1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_U1);
                return this;
            }
        }

        public EmitHelper conv_u2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_U2);
                return this;
            }
        }

        public EmitHelper conv_u4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_U4);
                return this;
            }
        }

        public EmitHelper conv_u8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Conv_U8);
                return this;
            }
        }

        public EmitHelper cpblk
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Cpblk);
                return this;
            }
        }

        public EmitHelper div
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Div);
                return this;
            }
        }

        public EmitHelper div_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Div_Un);
                return this;
            }
        }

        public EmitHelper dup
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Dup);
                return this;
            }
        }

        public EmitHelper endfilter
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Endfilter);
                return this;
            }
        }

        public EmitHelper endfinally
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Endfinally);
                return this;
            }
        }

        public System.Reflection.Emit.ILGenerator ILGenerator
        {
            get
            {
                return this._ilGenerator;
            }
        }

        public EmitHelper initblk
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Initblk);
                return this;
            }
        }

        public EmitHelper ldarg_0
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldarg_0);
                return this;
            }
        }

        public EmitHelper ldarg_1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldarg_1);
                return this;
            }
        }

        public EmitHelper ldarg_2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldarg_2);
                return this;
            }
        }

        public EmitHelper ldarg_3
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldarg_3);
                return this;
            }
        }

        public EmitHelper ldc_i4_0
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_0);
                return this;
            }
        }

        public EmitHelper ldc_i4_1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_1);
                return this;
            }
        }

        public EmitHelper ldc_i4_2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_2);
                return this;
            }
        }

        public EmitHelper ldc_i4_3
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_3);
                return this;
            }
        }

        public EmitHelper ldc_i4_4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_4);
                return this;
            }
        }

        public EmitHelper ldc_i4_5
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_5);
                return this;
            }
        }

        public EmitHelper ldc_i4_6
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_6);
                return this;
            }
        }

        public EmitHelper ldc_i4_7
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_7);
                return this;
            }
        }

        public EmitHelper ldc_i4_8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_8);
                return this;
            }
        }

        public EmitHelper ldc_i4_m1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldc_I4_M1);
                return this;
            }
        }

        public EmitHelper ldelem_i
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_I);
                return this;
            }
        }

        public EmitHelper ldelem_i1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_I1);
                return this;
            }
        }

        public EmitHelper ldelem_i2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_I2);
                return this;
            }
        }

        public EmitHelper ldelem_i4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_I4);
                return this;
            }
        }

        public EmitHelper ldelem_i8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_I8);
                return this;
            }
        }

        public EmitHelper ldelem_r4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_R4);
                return this;
            }
        }

        public EmitHelper ldelem_r8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_R8);
                return this;
            }
        }

        public EmitHelper ldelem_ref
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_Ref);
                return this;
            }
        }

        public EmitHelper ldelem_u1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_U1);
                return this;
            }
        }

        public EmitHelper ldelem_u2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_U2);
                return this;
            }
        }

        public EmitHelper ldelem_u4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldelem_U4);
                return this;
            }
        }

        public EmitHelper ldind_i
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_I);
                return this;
            }
        }

        public EmitHelper ldind_i1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_I1);
                return this;
            }
        }

        public EmitHelper ldind_i2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_I2);
                return this;
            }
        }

        public EmitHelper ldind_i4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_I4);
                return this;
            }
        }

        public EmitHelper ldind_i8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_I8);
                return this;
            }
        }

        public EmitHelper ldind_r4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_R4);
                return this;
            }
        }

        public EmitHelper ldind_r8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_R8);
                return this;
            }
        }

        public EmitHelper ldind_ref
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_Ref);
                return this;
            }
        }

        public EmitHelper ldind_u1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_U1);
                return this;
            }
        }

        public EmitHelper ldind_u2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_U2);
                return this;
            }
        }

        public EmitHelper ldind_u4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldind_U4);
                return this;
            }
        }

        public EmitHelper ldlen
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldlen);
                return this;
            }
        }

        public EmitHelper ldloc_0
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldloc_0);
                return this;
            }
        }

        public EmitHelper ldloc_1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldloc_1);
                return this;
            }
        }

        public EmitHelper ldloc_2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldloc_2);
                return this;
            }
        }

        public EmitHelper ldloc_3
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldloc_3);
                return this;
            }
        }

        public EmitHelper ldnull
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Ldnull);
                return this;
            }
        }

        public EmitHelper localloc
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Localloc);
                return this;
            }
        }

        public EmitHelper mul
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Mul);
                return this;
            }
        }

        public EmitHelper mul_ovf
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Mul_Ovf);
                return this;
            }
        }

        public EmitHelper mul_ovf_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Mul_Ovf_Un);
                return this;
            }
        }

        public EmitHelper neg
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Neg);
                return this;
            }
        }

        public EmitHelper nop
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Nop);
                return this;
            }
        }

        public EmitHelper not
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Not);
                return this;
            }
        }

        public EmitHelper or
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Or);
                return this;
            }
        }

        public EmitHelper pop
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Pop);
                return this;
            }
        }

        public EmitHelper refanytype
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Refanytype);
                return this;
            }
        }

        public EmitHelper rem
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Rem);
                return this;
            }
        }

        public EmitHelper rem_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Rem_Un);
                return this;
            }
        }

        public EmitHelper rethrow
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Rethrow);
                return this;
            }
        }

        public EmitHelper shl
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Shl);
                return this;
            }
        }

        public EmitHelper shr
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Shr);
                return this;
            }
        }

        public EmitHelper shr_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Shr_Un);
                return this;
            }
        }

        public EmitHelper stelem_i
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_I);
                return this;
            }
        }

        public EmitHelper stelem_i1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_I1);
                return this;
            }
        }

        public EmitHelper stelem_i2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_I2);
                return this;
            }
        }

        public EmitHelper stelem_i4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_I4);
                return this;
            }
        }

        public EmitHelper stelem_i8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_I8);
                return this;
            }
        }

        public EmitHelper stelem_r4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_R4);
                return this;
            }
        }

        public EmitHelper stelem_r8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_R8);
                return this;
            }
        }

        public EmitHelper stelem_ref
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stelem_Ref);
                return this;
            }
        }

        public EmitHelper stind_i
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_I);
                return this;
            }
        }

        public EmitHelper stind_i1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_I1);
                return this;
            }
        }

        public EmitHelper stind_i2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_I2);
                return this;
            }
        }

        public EmitHelper stind_i4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_I4);
                return this;
            }
        }

        public EmitHelper stind_i8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_I8);
                return this;
            }
        }

        public EmitHelper stind_r4
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_R4);
                return this;
            }
        }

        public EmitHelper stind_r8
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_R8);
                return this;
            }
        }

        public EmitHelper stind_ref
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stind_Ref);
                return this;
            }
        }

        public EmitHelper stloc_0
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stloc_0);
                return this;
            }
        }

        public EmitHelper stloc_1
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stloc_1);
                return this;
            }
        }

        public EmitHelper stloc_2
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stloc_2);
                return this;
            }
        }

        public EmitHelper stloc_3
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Stloc_3);
                return this;
            }
        }

        public EmitHelper sub
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Sub);
                return this;
            }
        }

        public EmitHelper sub_ovf
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Sub_Ovf);
                return this;
            }
        }

        public EmitHelper sub_ovf_un
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Sub_Ovf_Un);
                return this;
            }
        }

        public EmitHelper tailcall
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Tailcall);
                return this;
            }
        }

        public EmitHelper @throw
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Throw);
                return this;
            }
        }

        public EmitHelper Volatile
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Volatile);
                return this;
            }
        }

        public EmitHelper xor
        {
            get
            {
                this._ilGenerator.Emit(OpCodes.Xor);
                return this;
            }
        }
    }
}

