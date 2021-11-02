import React from "react";
import axios from "axios";
import { Redirect } from "react-router";

class AccountSettings extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      deleted: false
    }
  }
  handleDelete() {
    if (!window.confirm(`Are you sure you want to delete your account?`))
      return;

    const _this = this;

    const config = {
			headers: { Authorization: `Bearer ${this.props.user.token}` }
		}

    console.log(this.props.user);

    axios.delete(`https://localhost:5001/api/User`, config).then(function(response) {
      _this.setState({ deleted: true })
      }).catch((error) => {
        console.log(error);
    })
  }

  render() {
    return(
      <div className="account-settings">
        {this.state.deleted ? 
          <Redirect to="/login" /> : null}
        <h2>Account settings</h2>
        <p onClick={this.handleDelete.bind(this)}>Delete your account</p>
      </div>
    )
  }
}

export default AccountSettings;