﻿/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the Apache License, Version 2.0, please send an email to 
 * vspython@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Utilities;

namespace TestUtilities.Mocks {
    public class MockComponentModel : IComponentModel {
        public readonly Dictionary<Type, List<Lazy<object>>> Extensions = new Dictionary<Type, List<Lazy<object>>>();

        public void AddExtension<T>(Func<T> creator) where T : class {
            AddExtension(typeof(T), creator);
        }

        public void AddExtension<T>(Type key, Func<T> creator) where T : class {
            List<Lazy<object>> extensions;
            if (!Extensions.TryGetValue(key, out extensions)) {
                Extensions[key] = extensions = new List<Lazy<object>>();
            }
            extensions.Add(new Lazy<object>(creator));
        }

        public T GetService<T>() where T : class {
            List<Lazy<object>> extensions;
            if (Extensions.TryGetValue(typeof(T), out extensions)) {
                Debug.Assert(extensions.Count == 1, "Multiple extensions were registered");
                return (T)extensions[0].Value;
            }
            Console.WriteLine("Unregistered component model service " + typeof(T).FullName);
            return null;
        }

        public System.ComponentModel.Composition.Primitives.ComposablePartCatalog DefaultCatalog {
            get { throw new NotImplementedException(); }
        }

        public System.ComponentModel.Composition.ICompositionService DefaultCompositionService {
            get { throw new NotImplementedException(); }
        }

        public System.ComponentModel.Composition.Hosting.ExportProvider DefaultExportProvider {
            get { throw new NotImplementedException(); }
        }

        public System.ComponentModel.Composition.Primitives.ComposablePartCatalog GetCatalog(string catalogName) {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetExtensions<T>() where T : class {
            yield break;
        }
    }
}
