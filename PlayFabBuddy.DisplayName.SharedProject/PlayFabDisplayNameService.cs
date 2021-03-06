﻿using PlayFab.ClientModels;
using PlayFabBuddyLib.Auth;
using System.Threading.Tasks;

namespace PlayFabBuddyLib.DisplayName
{
	public class PlayFabDisplayNameService : IPlayFabDisplayNameService
	{
		#region Events

		public delegate void DisplayNameChangeEvent(string displayName);

		public event DisplayNameChangeEvent OnDisplayNameChange;

		#endregion //Events

		#region Properties

		private string _displayName;

		IPlayFabClient _playfab;
		IPlayFabAuthService _auth;

		#endregion //Properties

		#region Methods

		public PlayFabDisplayNameService(IPlayFabClient playfab, IPlayFabAuthService auth)
		{
			_playfab = playfab;
			_auth = auth;
		}

		public async Task<string> GetDisplayName()
		{
			var result = await _playfab.GetAccountInfoAsync(new GetAccountInfoRequest()
			{
				PlayFabId = _auth.PlayFabId
			}).ConfigureAwait(false);

			if (null == result.Error)
			{
				_displayName = result.Result?.AccountInfo?.TitleInfo?.DisplayName;
			}
			return _displayName;
		}

		public async Task<string> SetDisplayName(string displayName)
		{
			var result = await _playfab.UpdateUserTitleDisplayNameAsync(new UpdateUserTitleDisplayNameRequest()
			{
				DisplayName = displayName
			}).ConfigureAwait(false);

			OnDisplayNameChange?.Invoke(displayName);

			return result.Error?.ErrorMessage ?? string.Empty;
		}

		#endregion //Methods
	}
}
