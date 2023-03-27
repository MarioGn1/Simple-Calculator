import { useState } from 'react';
import { Alert } from 'react-bootstrap';
import * as httpClient from "../shared/httpClient"

const url = 'https://localhost:5002/api/calculator/calculate';

const Edit = () => {
    const [errors, setErrors] = useState({ description: false })
    const [result, setResult] = useState({ data: false })

    const submitHandler = async (e) => {
        e.preventDefault();

        let expression = Object.fromEntries(new FormData(e.currentTarget));
        let response = await httpClient.post(url, expression);

        if (response.ok) {
            let respResult = await response.json();
            setResult((prevState) => ({ ...prevState, data: respResult }));

            return;
        }

        let errorData = await response.json()
        setResult((prevState) => ({ ...prevState, data: false }));
        errorData = errorData.errors.Expression.join()

        setErrors(prevState => ({ ...prevState, description: errorData }))
    }

    const nameChangeHandler = (e) => {
        setResult((prevState) => ({ ...prevState, data: false }));

        let currentName = e.target.value;
        if (currentName.length <= 0) {
            setErrors(prevState => ({ ...prevState, description: 'Expression can\'t be empty!' }))
        } else if (currentName.length > 200) {
            setErrors(prevState => ({ ...prevState, description: 'Expression can\'t exceed 200 characters' }))
        } else {
            setErrors(prevState => ({ ...prevState, description: false }))
        }
    };

    return (
        <section id="calculate-page" className="calculate">
            <form id="calculate-form" method="POST" onSubmit={submitHandler}>
                <fieldset>
                    <p className="field">
                        <label htmlFor="expression">Expression</label>
                        <span className="input" style={{ borderColor: errors.description ? 'red' : 'inherit' }}>
                            <input type="text" name="expression" id="expression" onChange={nameChangeHandler} />
                        </span>
                        <Alert variant="danger" show={errors.description}>{errors.description}</Alert>
                    </p>
                    <input className="button submit" type="submit" value="Calculate" />
                    <p className="field" style={{ display: result.data || result.data === 0 ? 'block' : 'none' }}>
                        <label htmlFor="result">Result</label>
                        <span className="output" >
                            <mark id='result'>{result.data}</mark>
                        </span>
                    </p>
                </fieldset>
            </form>


        </section>
    );
}

export default Edit;