import React from "react";
import axios from "axios";
import GenericPage from "../shared/genericPage";
import ValidationError from "../shared/messages/validationError";
import ServerError from "../shared/messages/serverError";

class LoginPage extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
      fields: {
        "email": "", 
        "password":""
      },
      errors: {},
			mismatchMessage: "",
		};
	}

	handleChange(field, e) {
    let fields = this.state.fields;
    fields[field] = e.target.value;
    this.setState({ fields });
		this.setState({
      mismatchMessage: ""
    })
  }

	handleValidation() {
		let formValid = true;
    let errors = {};
    let fields = this.state.fields;

		if (!fields["email"]) {
      formValid = false;
      errors["email"] = "Enter your email address";
    } else if (fields["email"].length < 3 || 
							!fields["email"].includes('@') ||
							!fields["email"].includes('.')) {
      formValid = false;
      errors["email"] = "Enter a valid email addess";
    }

		if (!fields["password"]) {
      formValid = false;
      errors["password"] = "Enter your password";
    } else if (fields["password"].length < 8) {
      formValid = false;
      errors["password"] = "Enter a valid password";
    }

		this.setState({ errors: errors });
    return formValid;
	}

	tryLogin() {
		const _this = this;
		axios.post('https://localhost:5001/api/User/Login', this.state.fields)
		.then(function (response) {
			_this.props.handleLogin(response.data);
		})
		.catch(function (error) {
			_this.setState({ mismatchMessage: "Username and password do not match."});
			console.log(error);
		});
	}

	collectData(e) {
		e.preventDefault();

		if (!this.handleValidation())
      return;

		this.tryLogin();
	}

	render() {
		return(
			<GenericPage>
				<h2>Login to your account</h2>

				<label>Email address</label>
				<input 
					type="text"
					value={this.state.fields["email"]}
					onChange={this.handleChange.bind(this, "email")}
					maxLength="30"
				/>
				<ValidationError>{this.state.errors["email"]}</ValidationError>

				<label>Password</label>
				<input 
					type="password"
					value={this.state.fields["password"]}
					onChange={this.handleChange.bind(this, "password")}
					maxLength="30"
				/>
				<ValidationError>{this.state.errors["password"]}</ValidationError>

				<input type="submit" value="Login" onClick={ this.collectData.bind(this) }/>
				<ServerError>{this.state.mismatchMessage}</ServerError>
			</GenericPage>
		)
	}
}

export default LoginPage;