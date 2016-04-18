using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackupReport.Reports;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace BackupReport
{
    public class BackupReportRegistry : Registry
    {
        public BackupReportRegistry(string[] args)
        {
            Scan(
                assemblyScanner =>
                {
                    assemblyScanner.TheCallingAssembly();
                    assemblyScanner.Convention<ReportConvention>();
                });

            For<DirectoryInfo>().Use(new DirectoryInfo(args[1]));
            For<TextReader>().Use(() => new StreamReader(args[0]));
            For<TextWriter>().Use(() => new StreamWriter(args[2]));
            For<ICreateFileSize>().Use<FileSystemInfoFileSizeFactory>();
            For<IRead<IList<string>>>().Use<FileSystemInfoBackupTargetReader>();
            For<IRead<IList<BackupManifestItem>>>().Use<BackupManifestItemReader>()
                .Ctor<Action<string>>().Is(errorMessage => Console.Error.WriteLine(errorMessage));
        }

        private class ReportConvention : IRegistrationConvention
        {
            public void ScanTypes(TypeSet types, Registry registry)
            {
                Type genericReportRunnerType = typeof(ReportRunner<>);

                IEnumerable<Type> reportTypes = GetClosedConcreteTypes(types).Where(TypeIsReport);
                foreach (Type reportType in reportTypes)
                {
                    Type genericArgument = reportType.GetInterfaces().SelectMany(interfaceType => interfaceType.GetGenericArguments()).First();
                    Type closedReportRunnerType = genericReportRunnerType.MakeGenericType(genericArgument);
                    Type collectorType = GetClosedConcreteTypes(types).First(TypeIsCollectorFor(reportType.Namespace));

                    registry.For(reportType.GetInterfaces().First()).Add(reportType);
                    registry.For(collectorType.GetInterfaces().First()).Add(collectorType);
                    registry.For(typeof(IRunReport)).Add(closedReportRunnerType);
                }
            }

            private static IEnumerable<Type> GetClosedConcreteTypes(TypeSet types)
            {
                return types.FindTypes(TypeClassification.Closed | TypeClassification.Concretes);
            }

            private static Func<Type, bool> InterfaceCloses(Type openInterfaceType)
            {
                return interfaceType => interfaceType.GetGenericTypeDefinition() != null
                    && openInterfaceType == interfaceType.GetGenericTypeDefinition();
            }

            private Func<Type, bool> TypeIsCollectorFor(string @namespace)
            {
                return type => string.Equals(type.Namespace, @namespace, StringComparison.Ordinal)
                    && type.GetInterfaces().Any(InterfaceCloses(typeof(ICollectReportData<>)));
            }

            private static bool TypeIsReport(Type type)
            {
                return type.Namespace.StartsWith("BackupReport.Reports.")
                    && type.GetInterfaces().Any(InterfaceCloses(typeof(IOutputReport<>)));
            }
        }
    }
}
