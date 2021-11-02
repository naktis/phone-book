import React from "react";
import { Link } from 'react-router-dom'
import './shared.css';

class Header extends React.Component {
	render() {
		return(
			<div className="header">
					<h1>Phone Book</h1>
						<div className="header-data-div">
							<div className="user-data-div">
								{this.props.children}
							</div>
							{ this.props.loggedIn ? 
								<Link to="./">
									<img src="./logout.png" onClick={this.props.handleLogout}></img>
								</Link>
							 : null }
							
						</div>
			</div>
		)
	}
}

export default Header;

/*
					<Link to={{
						pathname: '/account',
						user: this.props.user,
						handleLogout: this.props.handleLogout
					}}> </Link>
*/ 