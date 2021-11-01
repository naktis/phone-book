import React from "react";
import GenericPage from "../shared/genericPage";
import { Redirect } from 'react-router-dom';

class HomePage extends React.Component {
	constructor(props) {
		super(props);
	}
	
	render() {
		return(
			<GenericPage>
				{this.props.location.user === undefined ? <Redirect to="/login" /> : <h2>Phone Entries</h2>}
			</GenericPage>
		)
	}
}

export default HomePage;