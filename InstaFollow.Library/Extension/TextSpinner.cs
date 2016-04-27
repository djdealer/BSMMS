using System;
using log4net;

namespace InstaFollow.Library.Extension
{
	/// <summary>
	/// Supports both Nested and Flat Spinning
	/// </summary>
	/// <example>
	/// TextSpinner.Spin("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
	/// </example>
	public class TextSpinner
	{
		private readonly ILog log = LogManager.GetLogger(typeof(TextSpinner));
		public IRandomizer randomizer = RealRandom.Instance;
		public long permutations = 1;
		private const char OpenBrace = '{';
		private const char CloseBrace = '}';
		private const char Delimiter = '|';

		public string Spin(string content)
		{
			// quick data sanity check
			if (content == null)
			{
				throw new ArgumentException("Text content to spin is required.");
			}

			// get index of the start and ending bracess
			var start = content.IndexOf(OpenBrace);
			var end = content.IndexOf(CloseBrace);

			// return the original content if:
			//  if there are no braces {} at all
			//  there is no start brace {
			//  the end is before the start } {
			if (start == -1 && end == -1 || start == -1 || end < start)
			{
				return content;
			}

			// replace first brace
			var substring = content.Substring(start + 1, content.Length - (start + 1));

			// recursion
			var rest = this.Spin(substring);
			end = rest.IndexOf(CloseBrace);

			// check for issues
			if (end == -1)
			{
				throw new FormatException("Unbalanced brace.");
			}

			// get spin options
			var options = rest.Substring(0, end).Split(Delimiter);

			// update permutations count
			this.permutations *= options.Length;

			// get random item
			var item = options[this.randomizer.Generate(options.Length)];

			// substitute content and recurse the rest
			return content.Substring(0, start) + item + this.Spin(rest.Substring(end + 1, rest.Length - (end + 1)));
		}

		/// <summary>
		/// Gets the permutation count for the text to spin.
		/// </summary>
		/// <param name="content">The text to spin.</param>
		/// <returns>Integer containing the permutation count.</returns>
		public long Permutations(string content)
		{
			this.permutations = 1;

			this.Spin(content);

			return this.permutations;
		}
	}
}