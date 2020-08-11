// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "Unneeded for Fx", Scope = "member", Target = "~M:EmailService.Extensions.HttpResponseExtensions.ExecHandler``2(Microsoft.AspNetCore.Http.HttpResponse,Microsoft.AspNetCore.Http.HttpRequest,System.Func{``0,``1})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "Unneeded for Fx", Scope = "member", Target = "~M:EmailService.Extensions.HttpResponseExtensions.ExecHandler``1(Microsoft.AspNetCore.Http.HttpResponse,System.Func{``0})~System.Threading.Tasks.Task")]
