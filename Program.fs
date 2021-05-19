// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Nethereum.Web3
open Nethereum.Web3.Accounts
open Nethereum.JsonRpc.Client
open Newtonsoft.Json
open System.Threading.Tasks

let hardhatPrivKey = "ac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80"
let alchemyKey = "<your_key>"

let hardhatURI = "http://localhost:8545"

let inline runNow task =
    task
    |> Async.AwaitTask
    |> Async.RunSynchronously

let inline runNowWithoutResult (task:Task) =
    task |> Async.AwaitTask |> Async.RunSynchronously

let printBlockNumber (web3:Web3) a = 
    web3.Eth.Blocks.GetBlockNumber.SendRequestAsync() |> runNow |> a |> printfn "%s"

type HardhatForkInput() =
    [<JsonProperty(PropertyName = "jsonRpcUrl")>]
    member val JsonRpcUrl = "" with get, set
    [<JsonProperty(PropertyName = "blockNumber")>]
    member val BlockNumber = 0UL with get, set

type HardhatResetInput() =
    [<JsonProperty(PropertyName = "forking")>]
    member val Forking = HardhatForkInput() with get, set

type HardhatReset(client) = 
    inherit RpcRequestResponseHandler<bool>(client, "hardhat_reset")

    member __.SendRequestAsync (input:HardhatResetInput) (id:obj) = base.SendRequestAsync(id, input);

[<EntryPoint>]
let main argv =
    let web3 = Web3(Account(hardhatPrivKey), hardhatURI)

    // print block number before hardhat_reset
    sprintf "Before: %A" |> printBlockNumber web3

    let input = HardhatResetInput(Forking=HardhatForkInput(BlockNumber=12330245UL,JsonRpcUrl=alchemyKey))
    HardhatReset(web3.Client).SendRequestAsync input null |> runNow |> printfn "result: %b"
    
    // print block number after hardhat_reset
    sprintf "After: %A" |> printBlockNumber web3
    
    0 // return an integer exit code