using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class Web3Manager : MonoBehaviour
{
  [SerializeField] private Button connectButton;
  [SerializeField] private Button disconnectButton;
  [SerializeField] private Button mintButton;
  [SerializeField] private TextMeshProUGUI statusText;
  [SerializeField] private TextMeshProUGUI addressText;
  [SerializeField] private TextMeshProUGUI balanceText;
  [SerializeField] private TMP_InputField mintAmountInput;

  [DllImport("__Internal")]
  private static extern void ConnectWallet();

  [DllImport("__Internal")]
  private static extern void DisconnectWallet();

  [DllImport("__Internal")]
  private static extern string GetConnectedAddress();

  [DllImport("__Internal")]
  private static extern void MintToken(int amount);

  [DllImport("__Internal")]
  private static extern void GetBalance();

  private void Start()
  {
    connectButton.onClick.AddListener(HandleConnectClick);
    disconnectButton.onClick.AddListener(HandleDisconnectClick);
    mintButton.onClick.AddListener(HandleMintClick);
    UpdateUI();
  }

  private void HandleConnectClick()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
            ConnectWallet();
#endif
  }

  private void HandleDisconnectClick()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
            DisconnectWallet();
#endif
  }

  private void HandleMintClick()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
            if(int.TryParse(mintAmountInput.text, out int amount))
            {
                MintToken(amount);
                statusText.text = "Minting...";
            }
            else
            {
                statusText.text = "Invalid amount";
            }
#endif
  }

  public void OnWalletConnected(string address)
  {
    Debug.Log($"Wallet connected: {address}");
    UpdateUI();
  }

  public void OnWalletDisconnected(string address)
  {
    Debug.Log($"Wallet disconnected");
    UpdateUI();
  }

  public void OnWalletError(string error)
  {
    Debug.LogError($"Wallet error: {error}");
    statusText.text = "Connection failed";
  }

  public void OnMintSuccess(string txHash)
  {
    Debug.Log($"Mint successful: {txHash}");
    statusText.text = "Mint successful!";
    GetBalance(); // Update balance after mint
  }

  public void OnMintError(string error)
  {
    Debug.LogError($"Mint error: {error}");
    statusText.text = "Mint failed";
  }

  public void OnBalanceReceived(string balance)
  {
    Debug.Log($"Balance: {balance}");
    balanceText.text = $"Balance: {balance} tokens";
  }

  public void OnBalanceError(string error)
  {
    Debug.LogError($"Balance error: {error}");
    statusText.text = "Balance check failed";
  }

  private void UpdateUI()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
            string address = GetConnectedAddress();
            bool isConnected = !string.IsNullOrEmpty(address);
            
            statusText.text = isConnected ? "Connected" : "Not connected";
            addressText.text = isConnected ? $"Address: {address}" : "";
            connectButton.interactable = !isConnected;
            mintButton.interactable = isConnected;

            if(isConnected)
            {
                GetBalance();
            }
#endif
  }
}