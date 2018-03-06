using Unity.Builder;
using Unity.Extension;

namespace UnityAutoMoq
{
    /// <summary>
    /// Provide extensions for Unity Auto Moq
    /// </summary>
    public class UnityAutoMoqExtension : UnityContainerExtension
    {
        private readonly UnityAutoMoqContainer _autoMoqContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAutoMoqExtension"/> class.
        /// </summary>
        /// <param name="autoMoqContainer">The auto moq container.</param>
        public UnityAutoMoqExtension(UnityAutoMoqContainer autoMoqContainer)
        {
            _autoMoqContainer = autoMoqContainer;
        }

        /// <summary>
        /// Initialize the <see cref="UnityAutoMoqExtension"/>.
        /// </summary>
        protected override void Initialize()
        {
            Context.Strategies.Add(new UnityAutoMoqBuilderStrategy(_autoMoqContainer), UnityBuildStage.PreCreation);
        }
    }
}