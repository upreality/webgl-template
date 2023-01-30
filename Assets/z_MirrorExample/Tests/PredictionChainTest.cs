using MirrorExample;
using NUnit.Framework;
using UniRx;

public class PredictionChainTest
{
    [Test]
    public void GetBeforeFirstTimestampTest()
    {
        var defaultState = new FloatState(0f);
        var chain = new PredictionChain<FloatState>(defaultState);
        chain.Set(1f, new FloatState(1f));
        chain.Set(2f, new FloatState(2f));
        var stateBeforeFirst = chain.Get(0f);
        Assert.AreEqual(defaultState, stateBeforeFirst);
    }

    [Test]
    public void GetIntermediateTimestampTest()
    {
        var defaultState = new FloatState(0f);
        var chain = new PredictionChain<FloatState>(defaultState);
        chain.Set(1f, new FloatState(1f));
        chain.Set(3f, new FloatState(3f));

        var intermediateState = chain.Get(2f);
        var awaitedIntermediateState = new FloatState(2f);
        Assert.AreEqual(intermediateState, awaitedIntermediateState);

        intermediateState = chain.Get(1.5f);
        awaitedIntermediateState = new FloatState(1.5f);
        Assert.AreEqual(intermediateState, awaitedIntermediateState);

        intermediateState = chain.Get(2.5f);
        awaitedIntermediateState = new FloatState(2.5f);
        Assert.AreEqual(intermediateState, awaitedIntermediateState);
    }

    [Test]
    public void GetAfterMaxTimestampTest()
    {
        var defaultState = new FloatState(0f);
        var chain = new PredictionChain<FloatState>(defaultState);
        chain.Set(1f, new FloatState(1f));
        chain.Set(2f, new FloatState(2f));
        var afterMaxState = chain.Get(3f);
        var awaitedAfterMaxState = new FloatState(2f);
        Assert.AreEqual(afterMaxState, awaitedAfterMaxState);
    }

    [Test]
    public void SetIntermediateTimestampTest()
    {
        var defaultState = new FloatState(0f);
        var chain = new PredictionChain<FloatState>(defaultState);

        chain.Set(1f, new FloatState(1f));
        chain.Set(3f, new FloatState(3f));

        chain.Set(2f, new FloatState(2f));
        var lastStateSubject = chain.LastState as BehaviorSubject<FloatState>;
        var lastState = lastStateSubject.Value;
        var expectedLastState = new FloatState(3f);
        Assert.AreEqual(expectedLastState, lastState);

        chain.Set(2f, new FloatState(3f));
        lastStateSubject = chain.LastState as BehaviorSubject<FloatState>;
        lastState = lastStateSubject.Value;
        expectedLastState = new FloatState(4f);
        Assert.AreEqual(expectedLastState, lastState);
    }

    [Test]
    public void SetBeforeMinTimestampTest()
    {
        var defaultState = new FloatState(1f);
        var chain = new PredictionChain<FloatState>(defaultState);

        chain.Set(2f, new FloatState(2f));
        chain.Set(3f, new FloatState(3f));

        chain.Set(1f, new FloatState(2f));
        var lastStateSubject = chain.LastState as BehaviorSubject<FloatState>;
        var lastState = lastStateSubject.Value;
        var expectedLastState = new FloatState(3f);
        Assert.AreEqual(expectedLastState, lastState);
    }

    [Test]
    public void SetAfterMaxTimestampTest()
    {
        var defaultState = new FloatState(0f);
        var chain = new PredictionChain<FloatState>(defaultState);

        chain.Set(1f, new FloatState(1f));
        chain.Set(2f, new FloatState(2f));

        chain.Set(3f, new FloatState(3f));
        var lastStateSubject = chain.LastState as BehaviorSubject<FloatState>;
        var lastState = lastStateSubject.Value;
        var expectedLastState = new FloatState(3f);
        Assert.AreEqual(expectedLastState, lastState);
    }

    private struct FloatState : IState<FloatState>
    {
        public readonly float State;

        public FloatState(float state)
        {
            State = state;
        }

        public FloatState GetDelta(FloatState state) => new(state.State - State);

        public FloatState Multiply(float multiplier) => new(State * multiplier);

        public FloatState ApplyDelta(FloatState delta) => new(State + delta.State);
    }
}