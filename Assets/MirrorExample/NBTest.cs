using Mirror;
using MirrorExample;

public class NBTest : NBTestBase<long, string>
{
    protected override string GetDefaultState()
    {
        return "";
    }

    protected override void Apply(string state)
    {
    }

    protected override long GetInput()
    {
        return 0;
    }

    public override string Simulate(string state, long input)
    {
        return "";
    }

    [Command]
    protected override void CmdHandleInput(long timestamp, byte[] inputBytes) =>
        base.CmdHandleInput(timestamp, inputBytes);

    [ClientRpc]
    protected override void RpcHandleResult(long timestamp, byte[] bytesState) =>
        base.RpcHandleResult(timestamp, bytesState);
}