namespace FluorineFx.IO.Bytecode.Lightweight
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using log4net;
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Security;
    using System.Security.Permissions;

    internal class AMF0ReflectionOptimizer : IReflectionOptimizer
    {
        private CreateInstanceInvoker _createInstanceMethod;
        private ReadDataInvoker _readDataMethod;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMF0ReflectionOptimizer));

        public AMF0ReflectionOptimizer(Type type, AMFReader reader, object instance)
        {
            this._createInstanceMethod = this.CreateCreateInstanceMethod(type);
            this._readDataMethod = this.CreateReadDataMethod(type, reader, instance);
        }

        private CreateInstanceInvoker CreateCreateInstanceMethod(Type type)
        {
            DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), null, type, true);
            ILGenerator iLGenerator = method.GetILGenerator();
            ConstructorInfo con = type.GetConstructor(EmitHelper.AnyVisibilityInstance, null, CallingConventions.HasThis, Type.EmptyTypes, null);
            iLGenerator.Emit(OpCodes.Newobj, con);
            iLGenerator.Emit(OpCodes.Ret);
            return (CreateInstanceInvoker) method.CreateDelegate(typeof(CreateInstanceInvoker));
        }

        public object CreateInstance()
        {
            return this._createInstanceMethod();
        }

        protected virtual ReadDataInvoker CreateReadDataMethod(Type type, AMFReader reader, object instance)
        {
            bool skipVisibility = SecurityManager.IsGranted(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
            DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(AMFReader), typeof(ClassDefinition) }, base.GetType(), skipVisibility);
            ILGenerator iLGenerator = method.GetILGenerator();
            LocalBuilder builder = iLGenerator.DeclareLocal(type);
            LocalBuilder builder2 = iLGenerator.DeclareLocal(typeof(byte));
            LocalBuilder builder3 = iLGenerator.DeclareLocal(typeof(string));
            LocalBuilder builder4 = iLGenerator.DeclareLocal(typeof(object));
            LocalBuilder builder5 = iLGenerator.DeclareLocal(typeof(int));
            LocalBuilder builder6 = iLGenerator.DeclareLocal(typeof(int));
            LocalBuilder builder7 = iLGenerator.DeclareLocal(typeof(object));
            LocalBuilder builder8 = iLGenerator.DeclareLocal(typeof(Type));
            EmitHelper emit = new EmitHelper(iLGenerator);
            ConstructorInfo constructorInfo = type.GetConstructor(EmitHelper.AnyVisibilityInstance, null, CallingConventions.HasThis, Type.EmptyTypes, null);
            MethodInfo methodInfo = typeof(AMFReader).GetMethod("AddReference");
            MethodInfo info3 = typeof(AMFReader).GetMethod("ReadString");
            MethodInfo info4 = typeof(AMFReader).GetMethod("ReadByte");
            emit.newobj(constructorInfo).stloc_0.ldarg_0.ldloc_0.callvirt(methodInfo).ldc_i4_0.stloc_1.ldnull.stloc_2.end();
            string memberName = reader.ReadString();
            for (byte i = reader.ReadByte(); i != 9; i = reader.ReadByte())
            {
                emit.ldarg_0.callvirt(info3).stloc_2.ldarg_0.callvirt(info4).stloc_1.end();
                object obj2 = reader.ReadData(i);
                reader.SetMember(instance, memberName, obj2);
                MemberInfo[] member = type.GetMember(memberName);
                if ((member == null) || (member.Length <= 0))
                {
                    throw new MissingMemberException(type.FullName, memberName);
                }
                this.GeneratePropertySet(emit, i, member[0]);
                memberName = reader.ReadString();
            }
            Label label = emit.DefineLabel();
            ConstructorInfo info5 = typeof(UnexpectedAMF).GetConstructor(EmitHelper.AnyVisibilityInstance, null, CallingConventions.HasThis, Type.EmptyTypes, null);
            emit.ldarg_0.callvirt(info3).stloc_2.ldarg_0.callvirt(info4).stloc_1.ldloc_1.ldc_i4_s(9).ceq.brtrue_s(label).newobj(info5).@throw.end();
            emit.MarkLabel(label).ldloc_0.ret();
            return (ReadDataInvoker) method.CreateDelegate(typeof(ReadDataInvoker));
        }

        private void GeneratePropertySet(EmitHelper emit, int typeCode, MemberInfo memberInfo)
        {
            Type propertyType = null;
            Label label2;
            Label label4;
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                propertyType = memberInfo.DeclaringType.GetProperty(memberInfo.Name).PropertyType;
            }
            if (memberInfo is FieldInfo)
            {
                propertyType = memberInfo.DeclaringType.GetField(memberInfo.Name).FieldType;
            }
            if (propertyType == null)
            {
                throw new ArgumentNullException(memberInfo.Name);
            }
            if (propertyType.IsPrimitive || (propertyType == typeof(decimal)))
            {
                TypeCode code = Type.GetTypeCode(propertyType);
                switch (code)
                {
                    case TypeCode.Boolean:
                    {
                        Label loc = emit.ILGenerator.DefineLabel();
                        label2 = emit.ILGenerator.DefineLabel();
                        emit.ldloc_1.brfalse_s(loc).ldloc_0.ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadBoolean")).GeneratePrimitiveCast(code).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(loc).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
                        break;
                    }
                    case TypeCode.Char:
                        label4 = emit.ILGenerator.DefineLabel();
                        label2 = emit.ILGenerator.DefineLabel();
                        emit.ldloc_1.ldc_i4(2).ceq.brfalse_s(label4).ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadString")).stloc_2.ldloc_2.brfalse_s(label4).ldloc_2.ldsfld(typeof(string).GetField("Empty")).call(typeof(string).GetMethod("op_Inequality", new Type[] { typeof(string), typeof(string) })).brfalse_s(label4).ldloc_0.ldloc_2.ldc_i4_0.callvirt(typeof(string).GetMethod("get_Chars", new Type[] { typeof(int) })).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label4).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
                        break;

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    {
                        Label label = emit.ILGenerator.DefineLabel();
                        label2 = emit.ILGenerator.DefineLabel();
                        emit.ldloc_1.ldc_i4_0.ceq.brfalse_s(label).ldloc_0.ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadDouble")).GeneratePrimitiveCast(code).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
                        break;
                    }
                }
            }
            else if (propertyType.IsEnum)
            {
                Label label5 = emit.ILGenerator.DefineLabel();
                label2 = emit.ILGenerator.DefineLabel();
                Label label6 = emit.ILGenerator.DefineLabel();
                if ((typeCode == 2) || (typeCode == 0))
                {
                    emit.ldloc_1.brfalse_s(label6).ldloc_1.ldc_i4(2).ceq.brfalse_s(label5).ldloc_0.ldtoken(propertyType).call(typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) })).ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadString")).ldc_i4_1.call(typeof(Enum).GetMethod("Parse", new Type[] { typeof(Type), typeof(string), typeof(bool) })).unbox_any(propertyType).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label6).ldloc_0.ldtoken(propertyType).call(typeof(Type).GetMethod("GetTypeFromHandle")).ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadDouble")).conv_i4.call(typeof(Enum).GetMethod("ToObject", new Type[] { typeof(Type), typeof(int) })).unbox_any(propertyType).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label5).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
                }
            }
            else if (propertyType == typeof(DateTime))
            {
                Label label7 = emit.ILGenerator.DefineLabel();
                label2 = emit.ILGenerator.DefineLabel();
                emit.ldloc_1.ldc_i4(11).ceq.brfalse_s(label7).ldloc_0.ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadDateTime")).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label7).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
            }
            else if (propertyType == typeof(string))
            {
                Label label8 = emit.ILGenerator.DefineLabel();
                Label label9 = emit.ILGenerator.DefineLabel();
                label2 = emit.ILGenerator.DefineLabel();
                Label label10 = emit.ILGenerator.DefineLabel();
                Label label11 = emit.ILGenerator.DefineLabel();
                emit.ldloc_1.ldc_i4(2).ceq.brtrue_s(label10).ldloc_1.ldc_i4(12).ceq.brtrue_s(label11).ldloc_1.ldc_i4(5).ceq.brtrue_s(label9).ldloc_1.ldc_i4(6).ceq.brtrue_s(label9).br_s(label8).MarkLabel(label10).ldloc_0.ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadString")).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label11).ldloc_0.ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadLongString")).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label9).ldloc_0.ldc_i4_0.GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label8).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
            }
            else if (propertyType == typeof(Guid))
            {
                label4 = emit.ILGenerator.DefineLabel();
                label2 = emit.ILGenerator.DefineLabel();
                emit.ldloc_1.ldc_i4(2).ceq.brfalse_s(label4).ldloc_0.ldarg_0.callvirt(typeof(AMFReader).GetMethod("ReadString")).newobj(typeof(Guid).GetConstructor(EmitHelper.AnyVisibilityInstance, null, CallingConventions.HasThis, new Type[] { typeof(string) }, null)).GenerateSetMember(memberInfo).br_s(label2).MarkLabel(label4).GenerateThrowUnexpectedAMFException(memberInfo).MarkLabel(label2).end();
            }
            else
            {
                if (propertyType.IsValueType)
                {
                    throw new FluorineException("Struct value types are not supported");
                }
                emit.ldloc_0.ldarg_0.ldloc_1.callvirt(typeof(AMFReader).GetMethod("ReadData", new Type[] { typeof(byte) })).ldtoken(propertyType).call(typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) })).call(typeof(TypeHelper).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) })).CastFromObject(propertyType).GenerateSetMember(memberInfo).end();
            }
        }

        public virtual object ReadData(AMFReader reader, ClassDefinition classDefinition)
        {
            return this._readDataMethod(reader, classDefinition);
        }
    }
}

