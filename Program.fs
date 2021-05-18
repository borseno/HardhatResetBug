// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open Nethereum.Web3
open Nethereum.Web3.Accounts
open Nethereum.JsonRpc.Client
open Newtonsoft.Json
open System.Threading.Tasks

let hardhatPrivKey = "ac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80"
let alchemyKey = "<your_key>"

let hardhatURI = "http://localhost:8545"

type Forking() =
    member val jsonRpcUrl  = sprintf "https://eth-mainnet.alchemyapi.io/v2/%s" alchemyKey
    member val blockNumber = 12330245
type HardhatReset() =
    member val forking = Forking()

let inline runNow task =
    task
    |> Async.AwaitTask
    |> Async.RunSynchronously

let inline runNowWithoutResult (task:Task) =
    task |> Async.AwaitTask |> Async.RunSynchronously

let printBlockNumber (web3:Web3) a = 
    web3.Eth.Blocks.GetBlockNumber.SendRequestAsync() |> runNow |> a |> printfn "%s"

[<EntryPoint>]
let main argv =
    let web3 = Web3(Account(hardhatPrivKey), hardhatURI)

    // print block number before hardhat_reset
    sprintf "Before: %A" |> printBlockNumber web3

    // log what the serialization result is
    HardhatReset() |> JsonConvert.SerializeObject |> printfn "Sending JSON: %s"
    // send hardhat_reset request with a newly created HardhatReset object. [||] is an array. 
    web3.Client.SendRequestAsync(new RpcRequest(2, "hardhat_reset", [|HardhatReset()|])) |> runNowWithoutResult
    
    // print block number after hardhat_reset
    sprintf "After: %A" |> printBlockNumber web3
    
    0 // return an integer exit code