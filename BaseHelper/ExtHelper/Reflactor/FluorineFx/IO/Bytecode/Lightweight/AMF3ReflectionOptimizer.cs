namespace FluorineFx.IO.Bytecode.Lightweight
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using FluorineFx.IO.Bytecode;
    using log4net;
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Security;
    using System.Security.Permissions;

    internal class AMF3ReflectionOptimizer : IReflectionOptimizer
    {
        private ClassDefinition _classDefinition;
        private CreateInstanceInvoker _createInstanceMethod;
        private ReadDataInvoker _readDataMethod;
        private static readonly ILog log = LogManager.GetLogger(typeof(AMF3ReflectionOptimizer));

        public AMF3ReflectionOptimizer(Type type, ClassDefinition classDefinition, AMFReader reader, object instance)
        {
            this._classDefinition = classDefinition;
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
            MethodInfo methodInfo = typeof(AMFReader).GetMethod("AddAMF3ObjectReference");
            MethodInfo info3 = typeof(AMFReader).GetMethod("ReadByte");
            emit.newobj(constructorInfo).stloc_0.ldarg_0.ldloc_0.callvirt(methodInfo).ldc_i4_0.stloc_1.ldnull.stloc_2.end();
            for (int i = 0; i < this._classDefinition.MemberCount; i++)
            {
                string name = this._classDefinition.Members[i].Name;
                byte typeMarker = reader.ReadByte();
                object obj2 = reader.ReadAMF3Data(typeMarker);
                reader.SetMember(instance, name, obj2);
                emit.ldarg_0.callvirt(info3).stloc_1.end();
                MemberInfo[] member = type.GetMember(name);
                if ((member == null) || (member.Length <= 0))
                {
                    throw new MissingMemberException(type.FullName, name);
                }
                this.GeneratePropertySet(emit, typeMarker, member[0]);
            }
            Label loc = emit.DefineLabel();
            emit.MarkLabel(loc).ldloc_0.ret();
            return (ReadDataInvoker) method.CreateDelegate(typeof(ReadDataInvoker));
        }

        private void GeneratePropertySet(EmitHelper emit, int typeCode, MemberInfo memberInfo)
        {
            Type propertyType = null;
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
            emit.ldloc_0.ldarg_0.ldloc_1.callvirt(typeof(AMFReader).GetMethod("ReadAMF3Data", new Type[] { typeof(byte) })).ldtoken(propertyType).call(typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) })).call(typeof(TypeHelper).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) })).CastFromObject(propertyType).GenerateSetMember(memberInfo).end();
        }

        public object ReadData(AMFReader reader, ClassDefinition classDefinition)
        {
            return this._readDataMethod(reader, classDefinition);
        }
    }
}

