namespace Lensophy.Dto;

/// <summary>
/// Structure of the content to be analyzed.
/// </summary>
/// <param name="Message">The message to be analyzed.</param>
/// <param name="Context">The subject that should be taken into consideration during the response generation.</param>
/// <remarks>The use of <see cref="Context"/> aids in providing a subject-oriented response suggestion.
/// If omitted, the result will be disconnected from the subject, although the result will still make sense.</remarks>
/// <example><para><b>Context:</b> They are discussing the latest Indiana Jones movie.</para>
/// <para><b>Message:</b> "You are an idiot and your opinion is worthless. The old movies were better, you jerk!"</para>
/// <para><b>Possible result:</b> "Your opinion is valuable, but I personally prefer the older Indiana Jones movies."</para>
/// <para><b>Possible result with no context:</b> "Your opinion is valid, but the old film productions were more preferred."</para>
/// </example>
public readonly record struct ContentAnalyse(string Message, string? Context);